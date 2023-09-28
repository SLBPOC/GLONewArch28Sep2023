#pragma warning disable
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Delfi.Glo.DataAccess.Services
{
    public class WellHierarchyService : IWellHierarchyService
    {
        private readonly IWellService<WellDetailsDto> _wellService;
        private Dictionary<int, Node> flatnedHierarchy = new Dictionary<int, Node>();
        public IEnumerable<Node> Hierarchy;
        public WellHierarchyService(IWellService<WellDetailsDto> wellService)
        {
            this._wellService = wellService;
            this.Hierarchy = new List<Node>();
        }


        #region Public methods

        /// <summary>
        /// Get Well Hierarchy
        /// </summary>
        /// <returns>IEnumerable<Node> Object</returns>
        public async Task<IEnumerable<Node>> GetWellHierarchy()
        {
            IEnumerable<WellDto>? wells = await this._wellService.GetWells();
            if (wells != null)
            {
                this.Hierarchy = ProjectToWellHierarchy(wells);
                flatnedHierarchy = Flattened(Hierarchy).ToDictionary(node => node.NodeId);
                return Hierarchy;
            }
            return new List<Node>();

        }

        /// <summary>
        /// Search In well Hierarchy
        /// </summary>
        /// <param name="request">Search filter by</param>
        /// <returns>WellHierarchResponse object</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<WellHierarchResponse> SearchInWellHierarchy(WellHierarchyRequest request)
        {
            if (request.SearchLevels == null || request.SearchLevels.Count <= 0)
            {
                throw new InvalidOperationException("SearchLevels can not be empty");
            }
            var wells = await this._wellService.GetWells();
            if (wells != null)
            {
                Hierarchy = ProjectToWellHierarchy(wells);
                flatnedHierarchy = Flattened(Hierarchy).ToDictionary(node => node.NodeId);
                return new WellHierarchResponse()
                {
                    Hierarchy = Search(request.SearchText, request.SearchLevels.Distinct().ToArray())
                };
            }
            return new WellHierarchResponse() { };
        }


        /// <summary>
        /// Get Wells With Hierarchy
        /// </summary>
        /// <returns></returns>
        public async Task<WellHierarchResponse> GetWellsWithHierarchy()
        {
            return new WellHierarchResponse()
            {
                Hierarchy = await this.GetWellHierarchy()
            };
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Project To well Hierarchy
        /// </summary>
        /// <param name="wells"></param>
        /// <returns>IEnumerable<Node> Object </returns>
        private static IEnumerable<Node> ProjectToWellHierarchy(IEnumerable<WellDto> wells)
        {
            var hierarchy = new List<Node>();

            foreach (var item in wells)
            {
                // Find or create the field in the hierarchy
                var fieldNode = hierarchy.FirstOrDefault(node => node.NodeId == item.FieldId);
                if (fieldNode == null)
                {
                    fieldNode = PrepareNode(item.FieldName, item.FieldId, 0, NodeType.Field);
                    hierarchy.Add(fieldNode);
                }

                // Find or create the battery in the field
                var batteryNode = fieldNode.Children.FirstOrDefault(node => node.NodeId == item.BatteryId);
                if (batteryNode == null)
                {
                    batteryNode = PrepareNode(item.BatteryName, item.BatteryId, fieldNode.NodeId, NodeType.Battery);
                    fieldNode.Children.Add(batteryNode);
                }

                // Add the pad to the battery
                var padNode = batteryNode.Children.FirstOrDefault(node => node.NodeId == item.PadId);
                if (padNode == null)
                {
                    padNode = PrepareNode(item.PadName, item.PadId, batteryNode.NodeId, NodeType.Pad);
                    batteryNode.Children.Add(padNode);
                }

                padNode.Children.Add(PrepareNode(item.WellName, item.Id, padNode.NodeId, NodeType.Wells));
            }

            return hierarchy;
        }


        /// <summary>
        /// Prepare Node
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="type"></param>
        /// <returns>Node object</returns>
        private static Node PrepareNode(string name, int id, int parentId, NodeType type)
        {
            return new Node
            {
                Name = name,
                NodeId = id,
                NodeParentId = parentId,
                Type = type,
                Children = type == NodeType.Wells ? null : new List<Node>()
            };
        }

        /// <summary>
        /// Search Name
        /// </summary>
        /// <param name="x"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private static bool SearchName(Node x, string searchText)
        {
            return x.Name.ToLower().Contains(searchText.ToLower().Trim());
        }


        /// <summary>
        /// Flattened
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private static List<Node> Flattened(IEnumerable<Node> nodes)
        {
            List<Node> flattenedNodes = nodes
                .SelectMany(node => { Node cloneNode = (Node)node.Clone(); return new[] { cloneNode }.Concat(Flattened(cloneNode.Children ?? new List<Node>())); })
                .ToList();
            flattenedNodes.ForEach(node => node.Children?.Clear());
            return flattenedNodes;
        }

        /// <summary>
        /// Search-node
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private IEnumerable<Node> Search(string searchText, NodeType[] types)
        {
            List<Node> nodesListFromSearchText = flatnedHierarchy.Values
                .Where(x => types.Contains(x.Type) && SearchName(x, searchText))
                .ToList();

            int[] nodeTypesAfterSearch = nodesListFromSearchText.Select(x => (int)x.Type).Distinct().ToArray();
            IEnumerable<int> wells = new List<int>();
            IEnumerable<int> pads = new List<int>(); ;
            IEnumerable<int> batteries = new List<int>(); ;
            IEnumerable<int> fields = new List<int>(); ;
            IEnumerable<Node> wellNodes = nodesListFromSearchText.Where(x => x.Type == NodeType.Wells);
            IEnumerable<Node> padsNodes = nodesListFromSearchText.Where(x => x.Type == NodeType.Pad);
            IEnumerable<Node> batteryNodes = nodesListFromSearchText.Where(x => x.Type == NodeType.Battery);
            IEnumerable<Node> fieldsNodes = nodesListFromSearchText.Where(x => x.Type == NodeType.Field);
            UpdateWellHirarchyFromWells(nodeTypesAfterSearch, ref wells, ref pads, ref batteries, ref fields, wellNodes);
            UpdateWellHirarchyFromPads(nodeTypesAfterSearch, ref wells, ref pads, ref batteries, ref fields, padsNodes);
            UpdateWellHirarchyFromBattery(nodeTypesAfterSearch, ref wells, ref pads, ref batteries, ref fields, batteryNodes);
            UpdateWellHirarchyFromFields(nodeTypesAfterSearch, ref wells, ref pads, ref batteries, ref fields, fieldsNodes);
            return ProjectionAfterSearch(fields.Distinct().ToHashSet(), new HashSet<int>[] { batteries.Distinct().ToHashSet(), pads.Distinct().ToHashSet(), wells.Distinct().ToHashSet() }, NodeType.Field);

        }

        /// <summary>
        /// Update Well Hirarchy From Fields
        /// </summary>
        /// <param name="nodeTypesAfterSearch"></param>
        /// <param name="wells"></param>
        /// <param name="pads"></param>
        /// <param name="batteries"></param>
        /// <param name="fields"></param>
        /// <param name="fieldsNodes"></param>
        private void UpdateWellHirarchyFromFields(int[] nodeTypesAfterSearch, ref IEnumerable<int> wells, ref IEnumerable<int> pads, ref IEnumerable<int> batteries, ref IEnumerable<int> fields, IEnumerable<Node> fieldsNodes)
        {
            if (nodeTypesAfterSearch.Contains(0)) //for fields
            {
                //dig down as its Parent and top most in hierarchy

                fields = fields.Union(fieldsNodes.Select(field => field.NodeId).Distinct());

                var refFields = fields;
                var localBattery = this.flatnedHierarchy.Where(battery => battery.Value.Type == NodeType.Battery && refFields.Any(field => field == battery.Value.NodeParentId));
                batteries = batteries.Union(localBattery.Select(x => x.Key).Distinct());

                var localPads = this.flatnedHierarchy.Where(pad => pad.Value.Type == NodeType.Pad && localBattery.Any(battery => battery.Key == pad.Value.NodeParentId));
                pads = pads.Union(localPads.Select(pad => pad.Key).Distinct());

                var localWells = this.flatnedHierarchy.Where(well => well.Value.Type == NodeType.Wells && localPads.Any(pad => pad.Key == well.Value.NodeParentId));
                wells = wells.Union(localWells.Select(well => well.Key).Distinct());
            }
        }

        /// <summary>
        /// Update Well Hirarchy From Battery
        /// </summary>
        /// <param name="nodeTypesAfterSearch"></param>
        /// <param name="wells"></param>
        /// <param name="pads"></param>
        /// <param name="batteries"></param>
        /// <param name="fields"></param>
        /// <param name="batteryNodes"></param>
        private void UpdateWellHirarchyFromBattery(int[] nodeTypesAfterSearch, ref IEnumerable<int> wells, ref IEnumerable<int> pads, ref IEnumerable<int> batteries, ref IEnumerable<int> fields, IEnumerable<Node> batteryNodes)
        {
            if (nodeTypesAfterSearch.Contains(1)) //for battery
            {
                // dig down as its intermediate node
                var localBatteries = batteryNodes.Select(battery => battery.NodeId).Distinct();
                //batteries = localBatteries.Union(padsNodes.Select(pad => pad.NodeParentId != null ? (int)pad.NodeParentId : 0).Distinct());
                batteries = batteries.Union(localBatteries);
                var padsList = this.flatnedHierarchy.Where(pad => pad.Value.Type == NodeType.Pad && localBatteries.Any(battery => battery == pad.Value.NodeParentId));
                pads = pads.Union(padsList.Select(pad => pad.Key).Distinct());
                var extraWells = this.flatnedHierarchy.Where(well => well.Value.Type == NodeType.Wells && padsList.Select(pad => pad.Key).Distinct().Any(pad => pad == well.Value.NodeParentId));
                wells = wells.Union(extraWells.Select(well => well.Key).Distinct());
                // dig up as its intermediate node
                var localFields = this.flatnedHierarchy.Where(field => field.Value.Type == NodeType.Field && batteryNodes.Any(battery => battery.NodeParentId == field.Key));
                fields = fields.Union(localFields.Select(field => field.Key).Distinct());
            }
        }


        /// <summary>
        /// Update Well Hirarchy From Pads
        /// </summary>
        /// <param name="nodeTypesAfterSearch"></param>
        /// <param name="wells"></param>
        /// <param name="pads"></param>
        /// <param name="batteries"></param>
        /// <param name="fields"></param>
        /// <param name="padsNodes"></param>
        private void UpdateWellHirarchyFromPads(int[] nodeTypesAfterSearch, ref IEnumerable<int> wells, ref IEnumerable<int> pads, ref IEnumerable<int> batteries, ref IEnumerable<int> fields, IEnumerable<Node> padsNodes)
        {
            if (nodeTypesAfterSearch.Contains(2)) //for pads
            {
                // dig down as its intermediate node
                var padsList = padsNodes.Select(pad => pad.NodeId).Distinct();
                //pads = padsList.Union(wellNodes.Select(well => well.NodeParentId != null ? (int)well.NodeParentId : 0).Distinct());
                pads = pads.Union(padsList);
                var extraWells = this.flatnedHierarchy.Where(well => well.Value.Type == NodeType.Wells && padsList.Any(pad => pad == well.Value.NodeParentId));
                wells = wells.Union(extraWells.Select(well => well.Key).Distinct());
                // dig up as its intermediate node
                var localBattery = this.flatnedHierarchy.Where(battery => battery.Value.Type == NodeType.Battery && padsNodes.Any(pad => pad.NodeParentId == battery.Key));
                batteries = batteries.Union(localBattery.Select(battery => battery.Key).Distinct());
                var localFields = this.flatnedHierarchy.Where(fields => fields.Value.Type == NodeType.Field && localBattery.Any(battery => battery.Value.NodeParentId == fields.Key));
                fields = fields.Union(localFields.Select(fields => fields.Key).Distinct());

            }
        }


        /// <summary>
        /// Update Well Hirarchy From Wells
        /// </summary>
        /// <param name="nodeTypesAfterSearch"></param>
        /// <param name="wells"></param>
        /// <param name="pads"></param>
        /// <param name="batteries"></param>
        /// <param name="fields"></param>
        /// <param name="wellNodes"></param>
        private void UpdateWellHirarchyFromWells(int[] nodeTypesAfterSearch, ref IEnumerable<int> wells, ref IEnumerable<int> pads, ref IEnumerable<int> batteries, ref IEnumerable<int> fields, IEnumerable<Node> wellNodes)
        {
            if (nodeTypesAfterSearch.Contains(3)) // for wells
            {
                wells = wellNodes.Select(x => x.NodeId).Distinct();
                //dig up as its Leaf node
                var localPads = this.flatnedHierarchy.Where(pad => pad.Value.Type == NodeType.Pad && wellNodes.Any(well => well.NodeParentId == pad.Key));
                pads = localPads.Select(pad => pad.Key).Distinct();
                var localBattery = this.flatnedHierarchy.Where(battery => battery.Value.Type == NodeType.Battery && localPads.Any(pad => pad.Value.NodeParentId == battery.Key));
                batteries = localBattery.Select(battery => battery.Key).Distinct();
                var localFields = this.flatnedHierarchy.Where(fields => fields.Value.Type == NodeType.Field && localBattery.Any(battery => battery.Value.NodeParentId == fields.Key));
                fields = localFields.Select(x => x.Key).Distinct();

            }
        }


        /// <summary>
        /// Projectction After Search
        /// </summary>
        /// <param name="currentSet"></param>
        /// <param name="sets"></param>
        /// <param name="currentNode"></param>
        /// <returns>Node object</returns>
        private IEnumerable<Node> ProjectionAfterSearch(HashSet<int> currentSet, HashSet<int>[] sets, NodeType currentNode)
        {
            List<Node> result = new List<Node>();
            if (currentSet != null && currentSet.Count() > 0)
            {
                result = this.flatnedHierarchy.Keys.Intersect(currentSet).Select(x => this.flatnedHierarchy[x]).ToList();
                if (!result.Any())
                    result = this.flatnedHierarchy.Where(x => x.Value.Type == currentNode).Select(x => x.Value).ToList();
                if (((int)currentNode) <= 2)
                {

                    result.ForEach(x => x.Children = ProjectionAfterSearch(sets[(int)currentNode].Where(z => this.flatnedHierarchy[z].NodeParentId == x.NodeId).ToHashSet(), sets, (NodeType)currentNode + 1).ToList());
                }
            }
            return result;
        }

        #endregion


    }


















}
#pragma warning restore