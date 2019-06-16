using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.SharedLibs.Viewers.FileExplorer
{
    public partial class ScrapePage
    {
        public void InitializeStorage() => AppDatabase.Current.Init();
        public void DeleteStorage() => DBContext.Current.Manager.DeleteAllDatabases();
        public void ClearStorage()
        {
            try {
                DBContext.Current.DeleteAll<SavedVideo>();
            }
            catch (Exception ex) { }
            
        }

        public void Save(SavedVideo video) => DBContext.Current.Save(video);

        public void Delete(SavedVideo video) => DBContext.Current.DeleteEntity<SavedVideo>(video.UniqueId);

        public List<SavedVideo> RetrieveVideos() => DBContext.Current.RetrieveAllEntities<SavedVideo>();
        public (bool FoundAssets, List<SavedVideo> SavedVideos) RetrieveVideos(string guid)
        {
            if (DBContext.Current.DoesContextExist<SavedVideo>())
            {
                var foundAssets = DBContext.Current.RetrieveEntities<SavedVideo>($"grouping='{guid}'");
                if (foundAssets != null && foundAssets.Count > 0) return (true, foundAssets);
            }
            return (false, null);
        }

    }
}
