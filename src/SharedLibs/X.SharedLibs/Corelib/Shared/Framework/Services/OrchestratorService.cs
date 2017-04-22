
using SumoNinjaMonkey.Framework.Controls.Messages;
using SumoNinjaMonkey.Framework.Services;
using System.Linq;

namespace X.CoreLib.Shared.Services
{
    public class OrchestratorService
    {

        
        public void Orchestrate(GeneralSystemWideMessage msg)
        {
            if (msg.Identifier == "ORCHESTRATOR")
            {
                if (msg.Action == "MAKE EFFECT NON RENDERABLE")
                {
                    SaveIsRenderable(false, msg.AggregateId);
                }
                else if (msg.Action == "MAKE EFFECT RENDERABLE")
                {
                    SaveIsRenderable(true, msg.AggregateId);
                }
                else if (msg.Action == "DELETE AGGREGATE")
                {
                    AppDatabase.Current.DeleteUIElementState(msg.AggregateId);
                }
                else if (msg.Action == "MAKE TEXT NON RENDERABLE")
                {
                    SaveIsRenderable(false, msg.AggregateId);
                }
                else if (msg.Action == "MAKE TEXT RENDERABLE")
                {
                    SaveIsRenderable(true, msg.AggregateId);
                }
                else if (msg.Action == "MAKE VIDEO NON RENDERABLE")
                {
                    SaveIsRenderable(false, msg.AggregateId);
                }
                else if (msg.Action == "MAKE VIDEO RENDERABLE")
                {
                    SaveIsRenderable(true, msg.AggregateId);
                }
            }
        }




        private void SaveIsRenderable(bool isRenderable,string aggregateId )
        {
            if(!string.IsNullOrEmpty(aggregateId))
                AppDatabase.Current.UpdateUIElementStateField(aggregateId, "IsRenderable", isRenderable);
        }

    }
}
