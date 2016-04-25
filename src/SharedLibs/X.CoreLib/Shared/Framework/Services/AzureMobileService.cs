using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using SumoNinjaMonkey.Framework.Controls.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace FavouriteMX.Shared.Services
{

    public class AzureMobileService
    {
        private static AzureMobileService _azureMobileService = null;

        public static AzureMobileService Current
        {
            get
            {
                if (AzureMobileService._azureMobileService == null)
                {
                    AzureMobileService._azureMobileService = new AzureMobileService();
                }
                return AzureMobileService._azureMobileService;
            }
        }
        public static MobileServiceClient MobileService = new MobileServiceClient( AppService.AppSetting.AMSUrl );
        //"https://uapx.azurewebsites.net"

        private static IMobileServiceTable<Solution> mstSolution = MobileService.GetTable<Solution>();
        private static IMobileServiceTable<Project> mstProject = MobileService.GetTable<Project>();
        private static IMobileServiceTable<Scene> mstScene = MobileService.GetTable<Scene>();
        private static IMobileServiceTable<UIElementState> mstUIElementState = MobileService.GetTable<UIElementState>();
        private static IMobileServiceTable<Favourite> mstFavourite = MobileService.GetTable<Favourite>();
        private static IMobileServiceTable<Promote> mstPromote = MobileService.GetTable<Promote>();
        private static IMobileServiceTable<AppWideSetting> mstAppWideSetting = MobileService.GetTable<AppWideSetting>();
        private static IMobileServiceTable<Feedback> mstFeedback = MobileService.GetTable<Feedback>();
        private static IMobileServiceTable<Comment> mstComment = MobileService.GetTable<Comment>();

        private AzureMobileService()
        {
        }

        public void Unload()
        {
            
        }




        public async void DeleteFavouriteFromCloud(Favourite fav)
        {
            if (!AppService.IsConnected()) return;

            await mstFavourite.DeleteAsync(fav);
        }


        public async void PushToCloud(Project project)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if (project.MSId != 0)
                {
                    int id = project.Id;
                    project.Id = project.MSId;
                    project.MSId = id;
                    await mstProject.UpdateAsync(project);
                    project.Id = id;
                }
                else
                {
                    project.Id = 0;
                    await mstProject.InsertAsync(project);
                    AppDatabase.Current.UpdateProjectField(project.AggregateId, "MSId", project.Id, false);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(Scene scene)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if (scene.MSId != 0)
                {
                    int id = scene.Id;
                    scene.Id = scene.MSId;
                    scene.MSId = id;
                    await mstScene.UpdateAsync(scene);
                    scene.Id = id;
                }
                else
                {
                    scene.Id = 0;
                    await mstScene.InsertAsync(scene);
                    AppDatabase.Current.UpdateSceneField(scene.AggregateId, "MSId", scene.Id, false);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(Solution solution)
        {
            if (!AppService.IsConnected()) return;

            try{
                if (solution.MSId != 0)
                {
                    int id = solution.Id;
                    solution.Id = solution.MSId;
                    solution.MSId = id;
                    await mstSolution.UpdateAsync(solution);
                    solution.Id = id;
                }
                else
                {
                    solution.Id = 0;
                    await mstSolution.InsertAsync(solution);
                    AppDatabase.Current.UpdateSolutionField(solution.AggregateId, "MSId", solution.Id, false);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(UIElementState uiElementState)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if (uiElementState.MSId != 0)
                {
                    int id = uiElementState.Id;
                    uiElementState.Id = uiElementState.MSId;
                    uiElementState.MSId = id;
                    await mstUIElementState.UpdateAsync(uiElementState);
                    uiElementState.Id = id;
                }
                else
                {
                    uiElementState.Id = 0;
                    await mstUIElementState.InsertAsync(uiElementState);
                    AppDatabase.Current.UpdateSolutionField(uiElementState.AggregateId, "MSId", uiElementState.Id, false);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(Favourite favourite)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if ( !string.IsNullOrEmpty(favourite.MSId) )
                {
                    string id = favourite.Id;
                    favourite.Id = favourite.MSId;
                    favourite.MSId = id;
                    await mstFavourite.UpdateAsync(favourite);
                    favourite.Id = id;
                }
                else
                {
                    favourite.Id = string.Empty;
                    favourite.MSId = string.Empty;
                    await mstFavourite.InsertAsync(favourite);
                    //AppDatabase.Current.UpdateFavouriteField(favourite.AggregateId, "MSId", favourite.Id, false);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(Feedback feedback)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if (!string.IsNullOrEmpty(feedback.MSId))
                {
                    string id = feedback.Id;
                    feedback.Id = feedback.MSId;
                    feedback.MSId = id;
                    await mstFeedback.UpdateAsync(feedback);
                    feedback.Id = id;
                }
                else
                {
                    feedback.Id = string.Empty;
                    feedback.MSId = string.Empty;
                    await mstFeedback.InsertAsync(feedback);
                    //AppDatabase.Current.UpdateFavouriteField(favourite.AggregateId, "MSId", favourite.Id, false);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(Comment comment)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if (!string.IsNullOrEmpty(comment.MSId))
                {
                    string id = comment.Id;
                    comment.Id = comment.MSId;
                    comment.MSId = id;
                    await mstComment.UpdateAsync(comment);
                    comment.Id = id;
                }
                else
                {
                    comment.Id = string.Empty;
                    comment.MSId = string.Empty;
                    await mstComment.InsertAsync(comment);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(Promote promote)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if (!string.IsNullOrEmpty( promote.MSId) )
                {
                    string id = promote.Id;
                    promote.Id = promote.MSId;
                    promote.MSId = id;
                    await mstPromote.UpdateAsync(promote);
                    promote.Id = id;
                }
                else
                {
                    promote.Id = string.Empty;
                    promote.MSId = string.Empty;
                    await mstPromote.InsertAsync(promote);
                    //AppDatabase.Current.UpdateFavouriteField(favourite.AggregateId, "MSId", favourite.Id, false);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void PushToCloud(AppWideSetting appWideSetting)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                if (appWideSetting != null && !String.IsNullOrEmpty(appWideSetting.Id))
                {
                    await mstAppWideSetting.UpdateAsync(appWideSetting);
                }
                else
                {
                    appWideSetting.Id = string.Empty;
                    appWideSetting.MSId = string.Empty;
                    await mstAppWideSetting.InsertAsync(appWideSetting);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SaveSolutionsToCloud(string sessionId)
        {
            if (!AppService.IsConnected()) return;

            try{
                var items = AppDatabase.Current.RetrieveSolutionsByGrouping(sessionId);
                foreach (var item in items)
                {
                    AzureMobileService.Current.PushToCloud(item);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SaveProjectsToCloud(string solutionId)
        {
            if (!AppService.IsConnected()) return;

            try{
                var items = AppDatabase.Current.RetrieveProjectsByGrouping(solutionId);
                foreach (var item in items)
                {
                    AzureMobileService.Current.PushToCloud(item);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SaveScenesToCloud(string aggregateId)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                var items = AppDatabase.Current.RetrieveScenesByGrouping(aggregateId);
                foreach (var item in items)
                {
                    AzureMobileService.Current.PushToCloud(item);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SaveUIElementStatesToCloud(string sceneAggregateId)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                var items = AppDatabase.Current.RetrieveUIElementStatesByScene(sceneAggregateId);
                foreach (var item in items)
                {
                    AzureMobileService.Current.PushToCloud(item);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SaveFavouriteToCloud(Favourite fav)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                //var items = AppDatabase.Current.RetrieveSolutionsByGrouping(sessionId);
                //foreach (var item in items)
                //{
                AzureMobileService.Current.PushToCloud(fav);
                //}

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SaveFeedbackToCloud(Feedback fb)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                AzureMobileService.Current.PushToCloud(fb);
             
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SaveCommentToCloud(Comment comment)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                //var items = AppDatabase.Current.RetrieveSolutionsByGrouping(sessionId);
                //foreach (var item in items)
                //{
                AzureMobileService.Current.PushToCloud(comment);
                //}

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }

        public async void SavePromoteToCloud(Promote promote)
        {
            if (!AppService.IsConnected()) return;

            try
            {
                //var items = AppDatabase.Current.RetrieveSolutionsByGrouping(sessionId);
                //foreach (var item in items)
                //{
                AzureMobileService.Current.PushToCloud(promote);
                //}

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "WRITE" });
            }
            catch
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "CLOUD BAR", SourceId = "AzureMobileService", Action = "ERROR" });
            }
        }


        public async Task<List<Favourite>> RetrieveFavoritesFromCloudAsync(int pageSize = 20)
        {
            if (!AppService.IsConnected()) return null;
            
            var result = await mstFavourite.OrderByDescending(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result;
        }




        public async Task<List<Favourite>> RetrieveFavoritesGreaterThanDateFromCloudAsync(DateTime dateStamp, int pageSize = 20)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstFavourite.Where(x => x.TimeStamp > dateStamp).OrderBy(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result.OrderByDescending(x=>x.TimeStamp).ToList();

        }

        public async Task<List<Favourite>> RetrieveFavoritesLessThanDateFromCloudAsync(DateTime dateStamp, int pageSize = 20)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstFavourite.Where(x => x.TimeStamp < dateStamp).OrderByDescending(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result;

        }








        public async Task<List<AppWideSetting>> RetrieveAppWideSettingFromCloudAsync()
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstAppWideSetting.ToListAsync();

            return result;

        }











        public async Task<List<Promote>> RetrievePromotedFromCloudAsync(int pageSize = 30)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstPromote.OrderByDescending(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result;

        }

        

        public async Task<List<Promote>> RetrievePromotedGreaterThanDateFromCloudAsync(DateTime dateStamp, int pageSize = 30)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstPromote.Where(x => x.TimeStamp > dateStamp).OrderBy(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result.OrderByDescending(x => x.TimeStamp).ToList();

        }

        public async Task<List<Promote>> RetrievePromotedLessThanDateFromCloudAsync(DateTime dateStamp, int pageSize = 30)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstPromote.Where(x => x.TimeStamp < dateStamp).OrderByDescending(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result;

        }

















        public async Task<List<Comment>> RetrieveCommentFromCloudAsync(int pageSize = 30)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstComment.OrderByDescending(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result;

        }



        public async Task<List<Comment>> RetrieveCommentGreaterThanDateFromCloudAsync(DateTime dateStamp, int pageSize = 30)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstComment.Where(x => x.TimeStamp > dateStamp).OrderBy(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result.OrderByDescending(x => x.TimeStamp).ToList();

        }

        public async Task<List<Comment>> RetrieveCommentLessThanDateFromCloudAsync(DateTime dateStamp, int pageSize = 30)
        {
            if (!AppService.IsConnected()) return null;

            var result = await mstComment.Where(x => x.TimeStamp < dateStamp).OrderByDescending(x => x.TimeStamp).Take(pageSize).ToListAsync();

            return result;

        }
    }
}
