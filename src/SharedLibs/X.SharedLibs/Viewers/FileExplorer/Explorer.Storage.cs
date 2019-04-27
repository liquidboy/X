using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.Viewer.FileExplorer
{
    public partial class Explorer
    {
        public void InitializeStorage() => AppDatabase.Current.Init();
        public void DeleteStorage() => DBContext.Current.Manager.DeleteAllDatabases();
        public void ClearStorage()
        {
            try {
                DBContext.Current.DeleteAll<SavedFolder>();
                DBContext.Current.DeleteAll<SavedAsset>();
            }
            catch (Exception ex) { }
            
        }

        public void Save(SavedFolder folder) => DBContext.Current.Save(folder);
        public void Save(SavedAsset asset) => DBContext.Current.Save(asset);

        public void Delete(SavedFolder folder) => DBContext.Current.DeleteEntity<SavedFolder>(folder.UniqueId);
        public void Delete(SavedAsset asset) => DBContext.Current.DeleteEntity<SavedAsset>(asset.UniqueId);

        public List<SavedFolder> RetrieveFolders() => DBContext.Current.RetrieveAllEntities<SavedFolder>();
        public (bool FoundAssets, List<SavedAsset> SavedAssets) RetrieveAssets(string guid)
        {
            if (DBContext.Current.DoesContextExist<SavedAsset>())
            {
                var foundAssets = DBContext.Current.RetrieveEntities<SavedAsset>($"grouping='{guid}'");
                if (foundAssets != null) return (true, foundAssets);
            }
            return (false, null);
        }


        //public SavedFolder CreateNewFolder(string guid, string name, string category) => CreateOrUpdateFolder(guid, name, category);
        //public SavedFolder UpdateExistingFolder(string guid) => CreateOrUpdateFolder(guid, string.Empty, string.Empty);
        //private SavedFolder CreateOrUpdateFolder(string guid, string name, string grouping)
        //{
        //    var folder = new SavedFolder(name, DateTime.UtcNow, DateTime.UtcNow, grouping);
        //    folder.UniqueId = Guid.Parse(guid);
        //    if (string.IsNullOrEmpty(guid) || guid.Equals(Guid.Empty.ToString()))
        //    {
        //        if (string.IsNullOrEmpty(name)) folder.Name = "[no-name-set]";
        //        Save(folder);
        //    }
        //    return folder;
        //}


        //public SavedAsset CreateNewAsset(string guid, string name, string parentFolder) => CreateOrUpdateAsset(guid, name, parentFolder);
        //public SavedAsset UpdateExistingAsset(string guid) => CreateOrUpdateAsset(guid, string.Empty, string.Empty);
        //private SavedAsset CreateOrUpdateAsset(string guid, string name, string parentFolder)
        //{
        //    var asset = new SavedAsset(name, DateTime.UtcNow, DateTime.UtcNow, parentFolder);
        //    asset.UniqueId = Guid.Parse(guid);
        //    if (string.IsNullOrEmpty(guid) || guid.Equals(Guid.Empty.ToString()))
        //    {
        //        if (string.IsNullOrEmpty(name)) asset.Name = "[no-name-set]";
        //        Save(asset);
        //    }
        //    return asset;
        //}

    }
}
