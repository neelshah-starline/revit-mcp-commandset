﻿using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using RevitMCPCommandSet.Services;
using RevitMCPSDK.API.Base;

namespace RevitMCPCommandSet.Commands
{
    public class TagWallsCommand : ExternalEventCommandBase
    {
        private TagWallsEventHandler _handler => (TagWallsEventHandler)Handler;

        /// <summary>
        /// Command name
        /// </summary>
        public override string CommandName => "tag_all_walls";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uiApp">Revit UIApplication</param>
        public TagWallsCommand(UIApplication uiApp)
            : base(new TagWallsEventHandler(), uiApp)
        {
        }

        public override object Execute(JObject parameters, string requestId)
        {
            try
            {
                // Parse parameters
                bool useLeader = false;
                if (parameters["useLeader"] != null)
                {
                    useLeader = parameters["useLeader"].ToObject<bool>();
                }

                string tagTypeId = null;
                if (parameters["tagTypeId"] != null)
                {
                    tagTypeId = parameters["tagTypeId"].ToString();
                }

                // Set tagging parameters
                _handler.SetParameters(useLeader, tagTypeId);

                // Trigger external event and wait for completion
                if (RaiseAndWaitForCompletion(10000))
                {
                    return _handler.TaggingResults;
                }
                else
                {
                    throw new TimeoutException("Wall tagging operation timed out");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Wall tagging failed: {ex.Message}");
            }
        }
    }
}
