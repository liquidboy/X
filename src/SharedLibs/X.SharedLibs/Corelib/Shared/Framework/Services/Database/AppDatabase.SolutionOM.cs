
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using X.CoreLib.SQLite;
using SumoNinjaMonkey.Framework;
using SumoNinjaMonkey.Framework.Controls.Messages;
using SumoNinjaMonkey.Framework.Services;
using System.Linq;
using Windows.Foundation;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace X.CoreLib.Shared.Services
{
    public partial class AppDatabase
    {



        //ADD/UPDATE

        public void AddDashboardItem(int slotId, int left, int top, int width, int height, string title, string description, int column, int row)
        {
            LoggingService.LogInformation("writing to db 'TableDashboard'", "AppDatabase.AddDashboardItem");

            this.SqliteDb.Insert(new TableDasboard()
            {
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                Title = title,
                Description = description,
                Ordinal = slotId,
                Column = column,
                Row = row
            });

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "AddDashboardItem" });

        }
        public void AddFolderItem(string title, int ordinal, string metroIcon)
        {
            LoggingService.LogInformation("writing to db 'FolderItem'", "AppDatabase.AddFolderItem");
            this.SqliteDb.Insert(new FolderItem()
            {
                Title = title,
                MetroIcon = metroIcon,
                Ordinal = ordinal
            });
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "AddFolderItem" });

        }
        public void AddMenuItem(string title, int ordinal, string metroIcon, int groupId, string id1)
        {
            LoggingService.LogInformation("writing to db 'FolderItem'", "AppDatabase.AddMenuItem");
            this.SqliteDb.Insert(new MenuItem()
            {
                Title = title,
                MetroIcon = metroIcon,
                Ordinal = ordinal,
                GroupId = groupId,
                Id1 = id1
            });
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "AddMenuItem" });

        }
        public void AddAppState(string name, string value)
        {
            LoggingService.LogInformation("writing to db 'AppState'", "AppDatabase.AddAppState");

            var found = RetrieveAppState(name);
            if (found != null && found.Count() > 0)
            {
                found[0].Value = value;
                this.SqliteDb.Update(found[0]);
                //await mstSolution.UpdateAsync(solution);
            }
            else
            {
                this.SqliteDb.Insert(new AppState()
                {
                    Name = name,
                    Value = value
                });
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "AppState" });
        }
        public void AddCacheCallResponse(string url, string data, DateTime timeStamp)
        {
            LoggingService.LogInformation("writing to db 'CacheCallResponse'", "AppDatabase.CacheCallResponse");

            var found = RetrieveCacheCallResponse(url);
            if (found != null && found.Count() > 0)
            {
                found[0].Data = data;
                found[0].TimeStamp = timeStamp;
                found[0].Url = url;
                this.SqliteDb.Update(found[0]);
               
            }
            else
            {
                this.SqliteDb.Insert(new CacheCallResponse()
                { 
                    Data = data,
                    TimeStamp = timeStamp,
                    Url = url
                });
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "CacheCallResponse" });
        }
        private void AddUIElementState(string aggregateId, string scene, double left, double top, double scale, double width, double height, bool isRenderable, int? layoutStyle, int? layoutOrientation)
        {
            LoggingService.LogInformation("writing to db 'UIElementState'", "AppDatabase.AddUIElementState");
            this.SqliteDb.Insert(new UIElementState()
            {
                AggregateId = aggregateId,
                Scene = scene,
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                Scale = scale,
                IsRenderable = isRenderable,
                LayoutStyle = layoutStyle == null ? 0 : (int)layoutStyle,
                LayoutOrientation = layoutOrientation == null ? 0 : (int)layoutOrientation
            });
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "UIElementState" });
        }
        public void AddSearchRequest(SearchRequest searchRequest)
        {
            LoggingService.LogInformation("writing to db 'SearchRequest'", "AppDatabase.AddSearchRequest");

            var found = RetrieveSearchRequest(searchRequest.Term, searchRequest.Type);
            if (found != null && found.Count() > 0)
            {
                found[0].TimeStamp = searchRequest.TimeStamp;
                this.SqliteDb.Update(found[0]);
                //await mstSolution.UpdateAsync(solution);
            }
            else
            {
                this.SqliteDb.Insert(searchRequest);
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "SearchRequest" });
        }
        public void AddGroupsRequest(GroupsRequest groupsRequest)
        {
            LoggingService.LogInformation("writing to db 'GroupsRequest'", "AppDatabase.AddGroupsRequest");

            var found = RetrieveGroupsRequest(groupsRequest.Term, groupsRequest.Type);
            if (found != null && found.Count() > 0)
            {
                found[0].TimeStamp = groupsRequest.TimeStamp;
                this.SqliteDb.Update(found[0]);
                //await mstSolution.UpdateAsync(solution);
            }
            else
            {
                this.SqliteDb.Insert(groupsRequest);
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("inserting ...") { Identifier = "DB", SourceId = "SearchRequest" });
        }


        public void AddUpdateUIElementState(string aggregateId, string scene, double left, double top, double width, double height, double scale, bool isRenderable, int? layoutStyle, int? layoutOrientation, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'UIElementState'", "AppDatabase.AddUpdateUIElementState");
            var found = RetrieveUIElementState(aggregateId);

            if (found != null && found.Count() > 0)
            {
                found[0].Left = left;
                found[0].Top = top;
                found[0].Width = width;
                found[0].Height = height;
                found[0].Scale = scale;
                found[0].IsRenderable = isRenderable;
                found[0].Scene = scene;
                if (layoutStyle != null) found[0].LayoutStyle = (int)layoutStyle;
                if (layoutOrientation != null) found[0].LayoutOrientation = (int)layoutOrientation;

                this.SqliteDb.Update(found[0]);
            }
            else
            {
                AddUIElementState(aggregateId, scene, left, top, scale, width, height, isRenderable, layoutStyle, layoutOrientation);
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });

            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "UPDATED" });

        }
        public async void AddUpdateSolution(Solution solution)
        {
            LoggingService.LogInformation("writing to db 'Solution'", "AppDatabase.AddUpdateSolution");
            var found = RetrieveSolution(solution.AggregateId);

            if (found != null && found.Count() > 0)
            {
                this.SqliteDb.Update(solution);
                //await mstSolution.UpdateAsync(solution);
            }
            else
            {
                var newId = this.SqliteDb.Insert(solution);
                //await mstSolution.InsertAsync(solution);

            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "Solution" });

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = solution.AggregateId, Action = "UPDATED" });

        }
        public void AddUpdateProject(Project project)
        {
            LoggingService.LogInformation("writing to db 'Project'", "AppDatabase.AddUpdateProject");
            var found = RetrieveProject(project.AggregateId);

            if (found != null && found.Count() > 0)
            {
                this.SqliteDb.Update(project);
                //await mstProject.UpdateAsync(project);
            }
            else
            {
                var newId = this.SqliteDb.Insert(project);
                //await mstProject.InsertAsync(project);
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "Project" });

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = project.AggregateId, Action = "UPDATED" });

        }
        public async void AddUpdateScene(Scene scene)
        {
            LoggingService.LogInformation("writing to db 'Scene'", "AppDatabase.AddUpdateScene");
            var found = RetrieveScene(scene.AggregateId);

            if (found != null && found.Count() > 0)
            {
                this.SqliteDb.Update(scene);
                //await mstScene.UpdateAsync(scene);
            }
            else
            {
                var newId = this.SqliteDb.Insert(scene);
                //await mstScene.InsertAsync(scene);
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "Scene" });

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = scene.AggregateId, Action = "UPDATED" });

        }

        public void UpdateSolutionField(string aggregateId, string fieldName, object fieldValue, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'Solution'", "AppDatabase.UpdateSolutionField");
            this.SqliteDb.Execute("UPDATE Solution set " + fieldName + " = ? where aggregateId = ?", fieldValue, aggregateId);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "Solution" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "UPDATED" });
        }
        public void UpdateProjectField(string aggregateId, string fieldName, object fieldValue, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'Project'", "AppDatabase.UpdateProjectField");
            this.SqliteDb.Execute("UPDATE Project set " + fieldName + " = ? where aggregateId = ?", fieldValue, aggregateId);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "Project" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "UPDATED" });
        }
        public void UpdateSceneField(string aggregateId, string fieldName, object fieldValue, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'Scene'", "AppDatabase.UpdateSceneField");
            this.SqliteDb.Execute("UPDATE Scene set " + fieldName + " = ? where aggregateId = ?", fieldValue, aggregateId);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "Scene" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "UPDATED" });
        }

        public void UpdateUIElementStateField(string aggregateId, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'UIElementState'", "AppDatabase.UpdateUIElementStateField");
            this.SqliteDb.Execute("UPDATE UIElementState set " + fieldName + " = ? where aggregateId = ?", value, aggregateId);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "UPDATED" });


        }






        //DELETE
        public void DeleteDashboardItem(int? id)
        {

            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'DashboardItem'", "AppDatabase.DeleteDashboardItem");
                this.SqliteDb.Delete(new TableDasboard() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteDashboardItem" });
        }
        public void DeleteFolderItem(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'FolderItem'", "AppDatabase.DeleteFolderItem");
                this.SqliteDb.Delete(new FolderItem() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteFolderItem" });
        }
        public void DeleteMenuItem(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'MenuItem'", "AppDatabase.DeleteMenuItem");
                this.SqliteDb.Delete(new MenuItem() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteMenuItem" });
        }
        public void DeleteMenuItems()
        {
            LoggingService.LogInformation("delete * 'MenuItem'", "AppDatabase.DeleteMenuItems");
            this.SqliteDb.DeleteAll<MenuItem>();
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "MenuItems" });
        }
        public void DeleteAppState(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'AppState'", "AppDatabase.DeleteAppState");
                this.SqliteDb.Delete(new AppState() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "AppState" });
        }
        public void DeleteAppState(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                LoggingService.LogInformation("delete from db 'AppState'", "AppDatabase.DeleteAppState");
                this.SqliteDb.Execute("delete from AppState where name = ?", name);

            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "AppState" });
        }
        public void DeleteAppStates()
        {
            LoggingService.LogInformation("delete * 'AppState'", "AppDatabase.DeleteAppState");
            this.SqliteDb.DeleteAll<AppState>();
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "AppStates" });
        }
        public void DeleteCacheCallResponse(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'CacheCallResponse'", "AppDatabase.DeleteCacheCallResponse");
                this.SqliteDb.Delete(new CacheCallResponse() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "CacheCallResponse" });
        }
        public void DeleteCacheCallResponses()
        {
            LoggingService.LogInformation("delete * 'CacheCallResponse'", "AppDatabase.DeleteCacheCallResponse");
            this.SqliteDb.DeleteAll<CacheCallResponse>();
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "CacheCallResponses" });
        }
        public void DeleteUIElementState(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'UIElementState'", "AppDatabase.DeleteUIElementState");
                this.SqliteDb.Delete(new UIElementState() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "UIElementState" });
        }
        public void DeleteUIElementState(string aggregateId, bool sendAggregateDeleteMessage = true)
        {
            if (!string.IsNullOrEmpty(aggregateId))
            {
                LoggingService.LogInformation("delete from db 'UIElementState'", "AppDatabase.DeleteUIElementState");
                this.SqliteDb.Execute("delete from UIElementState where grouping1 = ?", aggregateId);
                this.SqliteDb.Execute("delete from UIElementState where grouping2 = ?", aggregateId);
                this.SqliteDb.Execute("delete from UIElementState where aggregateId = ?", aggregateId);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteSolution(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'Solution'", "AppDatabase.DeleteSolution");
                this.SqliteDb.Delete(new Solution() { Id = (int)id });
                //mstSolution.DeleteAsync(new Solution() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Solution" });
            //if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteSolution(string aggregateId, bool sendAggregateDeleteMessage = true)
        {
            if (!string.IsNullOrEmpty(aggregateId))
            {
                LoggingService.LogInformation("delete from db 'Solution'", "AppDatabase.DeleteSolution");

                var found = RetrieveSolution(aggregateId);
                //mstSolution.DeleteAsync(found.First());

                this.SqliteDb.Execute("delete from Solution where aggregateId = ?", aggregateId);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Solution" });
            if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteSolutions(string grouping1)
        {
            if (!string.IsNullOrEmpty(grouping1))
            {
                LoggingService.LogInformation("delete from db 'Solution'", "AppDatabase.DeleteSolutions");
                this.SqliteDb.Execute("delete from Solution where Grouping1 = ?", grouping1);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Solution" });
            //if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteProject(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'Project'", "AppDatabase.DeleteProject");
                this.SqliteDb.Delete(new Project() { Id = (int)id });
                //mstProject.DeleteAsync(new Project() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Project" });
            //if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteProject(string aggregateId, bool sendAggregateDeleteMessage = true)
        {
            if (!string.IsNullOrEmpty(aggregateId))
            {
                LoggingService.LogInformation("delete from db 'Project'", "AppDatabase.DeleteProject");

                var found = RetrieveProject(aggregateId);
                //mstProject.DeleteAsync(found.First());

                this.SqliteDb.Execute("delete from Project where aggregateId = ?", aggregateId);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Project" });
            if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteProjects(string grouping1)
        {
            if (!string.IsNullOrEmpty(grouping1))
            {
                LoggingService.LogInformation("delete from db 'Project'", "AppDatabase.DeleteProjects");
                this.SqliteDb.Execute("delete from Project where Grouping1 = ?", grouping1);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Project" });
            //if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteScene(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'Scene'", "AppDatabase.DeleteScene");
                this.SqliteDb.Delete(new Scene() { Id = (int)id });
                //mstScene.DeleteAsync(new Scene() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Scene" });
            //if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteScene(string aggregateId, bool sendAggregateDeleteMessage = true)
        {
            if (!string.IsNullOrEmpty(aggregateId))
            {
                LoggingService.LogInformation("delete from db 'Scene'", "AppDatabase.DeleteScene");

                var found = RetrieveScene(aggregateId);
                //mstScene.DeleteAsync(found.First());

                this.SqliteDb.Execute("delete from Scene where aggregateId = ?", aggregateId);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "Scene" });
            if (sendAggregateDeleteMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "AGGREGATE", AggregateId = aggregateId, Action = "DELETED" });
        }
        public void DeleteSearchRequest(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'SearchRequest'", "AppDatabase.DeleteSearchRequest");
                this.SqliteDb.Delete(new SearchRequest() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "SearchRequest" });
        }
        public void DeleteSearchRequests()
        {
            LoggingService.LogInformation("delete * 'SearchRequest'", "AppDatabase.DeleteSearchRequest");
            this.SqliteDb.DeleteAll<SearchRequest>();
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "SearchRequests" });
        }
        public void DeleteGroupsRequest(int? id)
        {
            if (id != null)
            {
                LoggingService.LogInformation("delete from db 'GroupsRequest'", "AppDatabase.DeleteGroupsRequest");
                this.SqliteDb.Delete(new GroupsRequest() { Id = (int)id });
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "GroupsRequest" });
        }
        public void DeleteGroupsRequests()
        {
            LoggingService.LogInformation("delete * 'GroupsRequest'", "AppDatabase.DeleteGroupsRequest");
            this.SqliteDb.DeleteAll<GroupsRequest>();
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "GroupsRequests" });
        }




        //RETRIEVE
        public List<TableDasboard> RetrieveDashboard()
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveDashboard" });
            LoggingService.LogInformation("retrieve from db 'TableDashboard'", "AppDatabase.RetrieveDashboard");
            return this.SqliteDb.Query<TableDasboard>("SELECT Id, Ordinal, Left, Top, Width, Height, Column, Row FROM TableDasboard");
        }
        public List<FolderItem> RetrieveFolders()
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveFolders" });
            LoggingService.LogInformation("retrieve from db 'FolderItem'", "AppDatabase.RetrieveFolders");
            return this.SqliteDb.Query<FolderItem>("SELECT Id, Ordinal, Title, MetroIcon FROM FolderItem");
        }
        public List<MenuItem> RetrieveMenuItems()
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveMenuItems" });
            LoggingService.LogInformation("retrieve from db 'FolderItem'", "AppDatabase.RetrieveMenuItems");
            return this.SqliteDb.Query<MenuItem>("SELECT Id, Ordinal, Title, MetroIcon, GroupId, Id1 FROM MenuItem");
        }
        public List<AppState> RetrieveAppStates()
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveAppStates" });
            LoggingService.LogInformation("retrieve from db 'AppState'", "AppDatabase.RetrieveAppStates");
            return this.SqliteDb.Query<AppState>("SELECT Id, Name, Value FROM AppState");
        }
        public List<CacheCallResponse> CacheCallResponses()
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveCacheCallResponses" });
            LoggingService.LogInformation("retrieve from db 'CacheCallResponse'", "AppDatabase.RetrieveCacheCallResponses");
            return this.SqliteDb.Query<CacheCallResponse>("SELECT Id, Url, Data, TimeStamp FROM CacheCallResponse");
        }
        public async Task<List<SearchRequest>> RetrieveSearchRequests(int limit)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveSearchRequests" });
            LoggingService.LogInformation("retrieve from db 'SearchRequest'", "AppDatabase.RetrieveSearchRequests");
            return this.SqliteDb.Query<SearchRequest>("SELECT * FROM SearchRequest order by TimeStamp desc LIMIT " + limit);
        }
        public async Task<List<GroupsRequest>> RetrieveGroupsRequests(int limit)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveGroupsRequests" });
            LoggingService.LogInformation("retrieve from db 'GroupsRequest'", "AppDatabase.RetrieveGroupsRequests");
            return this.SqliteDb.Query<GroupsRequest>("SELECT * FROM GroupsRequest order by TimeStamp desc LIMIT " + limit);
        }


        public List<FolderItem> RetrieveFolder(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveFolder" });
            LoggingService.LogInformation("retrieve from db 'FolderItem'", "AppDatabase.RetrieveFolder");
            return this.SqliteDb.Query<FolderItem>("SELECT Id, Ordinal, Title, MetroIcon FROM FolderItem WHERE Id = ?", id);
        }
        public List<AppState> RetrieveAppState(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveAppState" });
            LoggingService.LogInformation("retrieve from db 'AppState'", "AppDatabase.RetrieveAppState");
            return this.SqliteDb.Query<AppState>("SELECT Id, Name, Value FROM AppState WHERE Id = ?", id);
        }
        public List<AppState> RetrieveAppState(string name)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveAppState" });
            LoggingService.LogInformation("retrieve from db 'AppState'", "AppDatabase.RetrieveAppState");
            return this.SqliteDb.Query<AppState>("SELECT * FROM AppState WHERE Name = ?", name);
        }
        public List<CacheCallResponse> RetrieveCacheCallResponse(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveCacheCallResponse" });
            LoggingService.LogInformation("retrieve from db 'CacheCallResponse'", "AppDatabase.RetrieveCacheCallResponse");
            return this.SqliteDb.Query<CacheCallResponse>("SELECT Id, Url, Data, TimeStamp FROM CacheCallResponse WHERE Id = ?", id);
        }
        public List<CacheCallResponse> RetrieveCacheCallResponse(string url)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveCacheCallResponse" });
            LoggingService.LogInformation("retrieve from db 'CacheCallResponse'", "AppDatabase.RetrieveCacheCallResponse");
            return this.SqliteDb.Query<CacheCallResponse>("SELECT * FROM CacheCallResponse WHERE Url = ?", url);
        }
        public List<SearchRequest> RetrieveSearchRequest(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveSearchRequest" });
            LoggingService.LogInformation("retrieve from db 'SearchRequest'", "AppDatabase.RetrieveSearchRequest");
            return this.SqliteDb.Query<SearchRequest>("SELECT * FROM AppState WHERE Id = ?", id);
        }
        public List<SearchRequest> RetrieveSearchRequest(string term, int type)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveSearchRequest" });
            LoggingService.LogInformation("retrieve from db 'SearchRequest'", "AppDatabase.RetrieveSearchRequest");
            return this.SqliteDb.Query<SearchRequest>("SELECT * FROM SearchRequest WHERE Term = ? and Type = ?", term, type);
        }
        public List<GroupsRequest> RetrieveGroupsRequest(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveGroupsRequest" });
            LoggingService.LogInformation("retrieve from db 'GroupsRequest'", "AppDatabase.RetrieveGroupsRequest");
            return this.SqliteDb.Query<GroupsRequest>("SELECT * FROM AppState WHERE Id = ?", id);
        }
        public List<GroupsRequest> RetrieveGroupsRequest(string term, int type)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveGroupsRequest" });
            LoggingService.LogInformation("retrieve from db 'GroupsRequest'", "AppDatabase.RetrieveGroupsRequest");
            return this.SqliteDb.Query<GroupsRequest>("SELECT * FROM GroupsRequest WHERE Term = ? and Type = ?", term, type);
        }


        private const string _fields_UIElementState = "Id, AggregateId, Scene, Grouping1, Grouping2, Type, Left, Top, Width, Height, Scale, IsRenderable, LayoutStyle, LayoutOrientation, udfString1, udfString2, udfString3, udfString4, udfString5, udfDouble1, udfDouble2, udfDouble3, udfDouble4, udfDouble5, udfBool1, udfBool2, udfBool3, udfBool4, udfBool5, udfInt1, udfInt2, udfInt3, udfInt4, udfInt5";

        public List<UIElementState> RetrieveUIElementState(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveUIElementState" });
            LoggingService.LogInformation("retrieve from db 'UIElementState'", "AppDatabase.RetrieveUIElementState");
            return this.SqliteDb.Query<UIElementState>("SELECT " + _fields_UIElementState + "  FROM UIElementState WHERE Id = ?", id);
        }

        public List<UIElementState> RetrieveUIElementState(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveUIElementState" });
            LoggingService.LogInformation("retrieve from db 'UIElementState'", "AppDatabase.RetrieveUIElementState");
            return this.SqliteDb.Query<UIElementState>("SELECT " + _fields_UIElementState + " FROM UIElementState WHERE AggregateId = ?", aggregateId);
        }

        public List<UIElementState> RetrieveUIElementStatesByGrouping(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveUIElementStatesByGrouping" });
            LoggingService.LogInformation("retrieve from db 'UIElementState'", "AppDatabase.RetrieveUIElementStatesByGrouping");
            return this.SqliteDb.Query<UIElementState>("SELECT " + _fields_UIElementState + " FROM UIElementState WHERE Grouping1 = ?", aggregateId);
        }
        public List<UIElementState> RetrieveUIElementStatesByScene(string sceneAggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveUIElementStatesByScene" });
            LoggingService.LogInformation("retrieve from db 'UIElementState'", "AppDatabase.RetrieveUIElementStatesByScene");
            return this.SqliteDb.Query<UIElementState>("SELECT " + _fields_UIElementState + " FROM UIElementState WHERE Scene = ?", sceneAggregateId);
        }

        public List<Solution> RetrieveSolution(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveSolution" });
            LoggingService.LogInformation("retrieve from db 'Solution'", "AppDatabase.RetrieveSolution");
            return this.SqliteDb.Query<Solution>("SELECT * FROM Solution WHERE ID = ?", id);
        }
        public List<Solution> RetrieveSolution(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveSolution" });
            LoggingService.LogInformation("retrieve from db 'Solution'", "AppDatabase.RetrieveSolution");
            return this.SqliteDb.Query<Solution>("SELECT * FROM Solution WHERE AggregateId = ?", aggregateId);
        }
        public List<Solution> RetrieveSolutionsByGrouping(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveSolutionsByGrouping" });
            LoggingService.LogInformation("retrieve from db 'Solution'", "AppDatabase.RetrieveSolutionsByGrouping");
            return this.SqliteDb.Query<Solution>("SELECT * FROM Solution WHERE Grouping1 = ?", aggregateId);
        }


        public List<Project> RetrieveProject(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveProject" });
            LoggingService.LogInformation("retrieve from db 'Project'", "AppDatabase.RetrieveProject");
            return this.SqliteDb.Query<Project>("SELECT * FROM Project WHERE ID = ?", id);
        }
        public List<Project> RetrieveProject(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveProject" });
            LoggingService.LogInformation("retrieve from db 'Project'", "AppDatabase.RetrieveProject");
            return this.SqliteDb.Query<Project>("SELECT * FROM Project WHERE AggregateId = ?", aggregateId);
        }
        public List<Project> RetrieveProjectsByGrouping(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveProjectsByGrouping" });
            LoggingService.LogInformation("retrieve from db 'Project'", "AppDatabase.RetrieveProjectsByGrouping");
            return this.SqliteDb.Query<Project>("SELECT * FROM Project WHERE Grouping1 = ?", aggregateId);
        }
        public List<Scene> RetrieveScene(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveScene" });
            LoggingService.LogInformation("retrieve from db 'Scene'", "AppDatabase.RetrieveScene");
            return this.SqliteDb.Query<Scene>("SELECT * FROM Scene WHERE ID = ?", id);
        }
        public List<Scene> RetrieveScene(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveScene" });
            LoggingService.LogInformation("retrieve from db 'Scene'", "AppDatabase.RetrieveScene");
            return this.SqliteDb.Query<Scene>("SELECT * FROM Scene WHERE AggregateId = ?", aggregateId);
        }
        public List<Scene> RetrieveScenesByGrouping(string aggregateId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveScenesByGrouping" });
            LoggingService.LogInformation("retrieve from db 'Scene'", "AppDatabase.RetrieveScenesByGrouping");
            return this.SqliteDb.Query<Scene>("SELECT * FROM Scene WHERE Grouping1 = ?", aggregateId);
        }




        //save

        public void SaveAppState(string name, string value)
        {
            var found = RetrieveAppState(name);

            if (found != null && found.Count == 1)
            {
                found[0].Value = value;
                this.SqliteDb.Update(found[0]);
            }
            else
            {
                if (found != null && found.Count == 0)
                {
                    AddAppState(name, value);
                }
                else
                {
                    if (found != null) { 
                        foreach (var item in found)
                        {
                            DeleteAppState(item.Id);
                        }
                    }
                    AddAppState(name, value);
                    
                }
            }
        }
        public void SaveCacheCallResponse(string url, string data, DateTime timeStamp)
        {
            var found = RetrieveCacheCallResponse(url);

            if (found != null && found.Count == 1)
            {
                found[0].Data = data;
                found[0].TimeStamp = timeStamp;
                this.SqliteDb.Update(found[0]);
            }
            else
            {
                if (found != null && found.Count == 0)
                {
                    AddCacheCallResponse(url, data, timeStamp);
                }
                else
                {
                    if (found != null)
                    {
                        foreach (var item in found)
                        {
                            DeleteCacheCallResponse(item.Id);
                        }
                    }

                    AddCacheCallResponse(url, data, timeStamp);

                }
            }
        }
        public void SaveSearchRequest(SearchRequest searchRequest)
        {
            var found = RetrieveSearchRequest(searchRequest.Term, searchRequest.Type);

            if (found != null && found.Count == 1)
            {
                found[0].TimeStamp = searchRequest.TimeStamp;

                found[0].MediaDescription = searchRequest.MediaDescription;
                found[0].MediaTitle = searchRequest.MediaTitle;
                found[0].MediaLicense = searchRequest.MediaLicense;
                found[0].MediaUrlSmall = searchRequest.MediaUrlSmall;
                found[0].MediaUrlMedium = searchRequest.MediaUrlMedium;
                found[0].MediaUserAvatar = searchRequest.MediaUserAvatar;
                found[0].MediaUserName = searchRequest.MediaUserName;
                found[0].CacheCallResponseUrl = searchRequest.CacheCallResponseUrl;

                this.SqliteDb.Update(found[0]);
            }
            else
            {
                if (found != null && found.Count == 0)
                {
                    AddSearchRequest(searchRequest);
                }
                else
                {
                    if (found != null)
                    {
                        foreach (var item in found)
                        {
                            DeleteSearchRequest(item.Id);
                        }
                    }
                    AddSearchRequest(searchRequest);

                }
            }
        }
        public void SaveGroupsRequest(GroupsRequest groupsRequest)
        {
            var found = RetrieveGroupsRequest(groupsRequest.Term, groupsRequest.Type);

            if (found != null && found.Count == 1)
            {
                found[0].TimeStamp = groupsRequest.TimeStamp;

                found[0].MediaDescription = groupsRequest.MediaDescription;
                found[0].MediaTitle = groupsRequest.MediaTitle;
                found[0].MediaLicense = groupsRequest.MediaLicense;
                found[0].MediaUrlSmall = groupsRequest.MediaUrlSmall;
                found[0].MediaUrlMedium = groupsRequest.MediaUrlMedium;
                found[0].MediaUserAvatar = groupsRequest.MediaUserAvatar;
                found[0].MediaUserName = groupsRequest.MediaUserName;
                found[0].CacheCallResponseUrl = groupsRequest.CacheCallResponseUrl;
                found[0].EighteenPlus = groupsRequest.EighteenPlus;
                found[0].TopicCount = groupsRequest.TopicCount;
                found[0].PoolCount = groupsRequest.PoolCount;
                found[0].MemberCount = groupsRequest.MemberCount;

                this.SqliteDb.Update(found[0]);
            }
            else
            {
                if (found != null && found.Count == 0)
                {
                    AddGroupsRequest(groupsRequest);
                }
                else
                {
                    if (found != null)
                    {
                        foreach (var item in found)
                        {
                            DeleteGroupsRequest(item.Id);
                        }
                    }
                    AddGroupsRequest(groupsRequest);

                }
            }
        }



        //ROAMING STATE
        public void AddRoamingState(string name, string value)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[name] = value;
        }

        public string RetrieveRoamingState(string name, string value)
        {

            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey(name))
            {
                return roamingSettings.Values[name].ToString();
            }

            return string.Empty;
        }


    }




    public class TableDasboard
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public int Ordinal { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }

    public class FolderItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public int Ordinal { get; set; }
        public string MetroIcon { get; set; }

    }

    public class MenuItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int GroupId { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }
        public int Ordinal { get; set; }
        public string MetroIcon { get; set; }
        public string Id1 { get; set; }
        
    }


    public class AppState
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public string Value { get; set; }

    }

    public class UIElementState
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MSId { get; set; }

        public string AggregateId { get; set; }

        public string Scene { get; set; }
        public string Grouping1 { get; set; }
        public string Grouping2 { get; set; }

        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; }


        public bool IsRenderable { get; set; }
        public int LayoutStyle { get; set; }
        public int LayoutOrientation { get; set; }
        public int Type { get; set; }


        public string udfString1 { get; set; }
        public string udfString2 { get; set; }
        public string udfString3 { get; set; }
        public string udfString4 { get; set; }
        public string udfString5 { get; set; }

        public double udfDouble1 { get; set; }
        public double udfDouble2 { get; set; }
        public double udfDouble3 { get; set; }
        public double udfDouble4 { get; set; }
        public double udfDouble5 { get; set; }

        public bool udfBool1 { get; set; }
        public bool udfBool2 { get; set; }
        public bool udfBool3 { get; set; }
        public bool udfBool4 { get; set; }
        public bool udfBool5 { get; set; }

        public int udfInt1 { get; set; }
        public int udfInt2 { get; set; }
        public int udfInt3 { get; set; }
        public int udfInt4 { get; set; }
        public int udfInt5 { get; set; }

        public UIElementState()
        {
            AggregateId = "";
            Scene = "";
            Grouping1 = "";
            Grouping2 = "";

            udfString1 = "";
            udfString2 = "";
            udfString3 = "";
            udfString4 = "";
            udfString5 = "";
        }
    }

    public class Solution
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MSId { get; set; }

        public string AggregateId { get; set; }
        public string Grouping1 { get; set; }
        public string Grouping2 { get; set; }

        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float TranslationX { get; set; }
        public float TranslationY { get; set; }
        public float TranslationZ { get; set; }

        public Solution()
        {
            AggregateId = "";
            Grouping1 = "";
            Grouping2 = "";

            ScaleX = 1;
            ScaleY = 1;
        }
    }

    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MSId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string AggregateId { get; set; }
        public string Grouping1 { get; set; }
        public string Grouping2 { get; set; }

        public bool IsRenderable { get; set; }

        public float RotationY { get; set; }
        public float RotationX { get; set; }

        public float ScaleZ { get; set; }
        public float ScaleY { get; set; }
        public float ScaleX { get; set; }

        public float TranslationX { get; set; }
        public float TranslationY { get; set; }
        public float TranslationZ { get; set; }

        public float Thickness { get; set; }

        public int Type { get; set; }

        public string PathResource { get; set; }
        public string PathMenuTitle { get; set; }
        public string PathNotificationMsg { get; set; }

        public string AssetUrl { get; set; }

        public int Ordinal { get; set; }

        public Project()
        {
            Title = "";
            Description = "";
            AggregateId = "";
            Grouping1 = "";
            Grouping2 = "";
            PathResource = "";
            PathMenuTitle = "";
            PathNotificationMsg = "";
            AssetUrl = "";


        }
    }

    public class Scene
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MSId { get; set; }

        public string AggregateId { get; set; }
        public string Grouping1 { get; set; }
        public string Grouping2 { get; set; }

        public long Type { get; set; }


        public string PathResource { get; set; }
        public string PathMenuTitle { get; set; }
        public string PathNotificationMsg { get; set; }

        public float TranslationX { get; set; }
        public float TranslationY { get; set; }
        public float TranslationZ { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public float CurrentLeft { get; set; }

        public Scene()
        {
            AggregateId = "";
            Grouping1 = "";
            Grouping2 = "";
            PathResource = "";
            PathMenuTitle = "";
            PathNotificationMsg = "";
        }
    }
    public class Comment
    {
        public string Id { get; set; }
        public string MSId { get; set; }

        public string AggregateId { get; set; }
        public string Grouping1 { get; set; }
        public string Grouping2 { get; set; }

        public string MediaUrlSmall { get; set; }
        public string MediaUrlMedium { get; set; }
        public string MediaUserName { get; set; }
        public string MediaUserAvatar { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string UserRealName { get; set; }


        public string EntityId { get; set; }
        public int Type { get; set; }

        public string Text1 { get; set; }
        public string Text2 { get; set; }

        public DateTime TimeStamp { get; set; }

        public Comment()
        {
            Text1 = "";
            Text2 = "";

            AggregateId = "";
            Grouping1 = "";
            Grouping2 = "";

            MediaUrlSmall = "";
            MediaUrlMedium = "";
            MediaUserName = "";
            MediaUserAvatar = "";
            UserName = "";
            UserAvatar = "";
            UserRealName = "";

            EntityId = "";
           

        }
    }
    public class Favourite 
    {
        
        public string Id { get; set; }
        public string MSId { get; set; }

        public string MediaTitle { get; set; }
        public string MediaDescription { get; set; }

        public string AggregateId { get; set; }
        public string Grouping1 { get; set; }
        public string Grouping2 { get; set; }

        public bool IsRenderable { get; set; }

        public string MediaUrlSmall { get; set; }
        public string MediaUrlMedium { get; set; }
        public string MediaUserName { get; set; }
        public string MediaUserAvatar { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string UserRealName { get; set; }
        public string ExtraText1 { get; set; }

        public bool IsPaging { get; set; }

        public string EntityId { get; set; }

        public int Type { get; set; }

        public DateTime TimeStamp { get; set; }

        public string MediaLicense { get; set; }

        public Favourite()
        {
            MediaTitle = "";
            MediaDescription = "";
            AggregateId = "";
            Grouping1 = "";
            Grouping2 = "";
            MediaUrlSmall = "";
            MediaUrlMedium = "";
            MediaUserName = "";
            MediaUserAvatar = "";
            UserName = "";
            UserAvatar = "";
            UserRealName = "";
            EntityId = "";
            MediaLicense = "";
            IsPaging = false;
            ExtraText1 = "";
        }
    }


    public class Promote  : Favourite
    {
        
    }

    public class AppWideSetting
    {
        public string Id { get; set; }
        public string MSId { get; set; }

        public string FlickrKey { get; set; }
        public string FlickrSecret { get; set; }

        public string TwitterKey { get; set; }
        public string TwitterSecret { get; set; }

        public string FavouriteMXPictureFolder { get; set; }
        public string WatchMXVideoFolder { get; set; }
        public string ExplorerMXDocumentsFolder { get; set; }
        public string AMSUrl { get; set; }
        public string AMSKey { get; set; }


        public AppWideSetting()
        {
            FlickrKey = "";
            FlickrSecret = "";
            TwitterKey = "";
            TwitterSecret = "";
            FavouriteMXPictureFolder = "";
            WatchMXVideoFolder = "";
            ExplorerMXDocumentsFolder = "";
            AMSUrl = "http://uapx.azurewebsites.net";
            AMSKey = "";
            
        }
    }


    public class Feedback
    {
        public string Id { get; set; }
        public string MSId { get; set; }

        public string Text { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public int statusId { get; set; }


        public Feedback()
        {
            Text = "";
            userId = "";
            userName = "";
            statusId = 0;
        }
    }

    public class CacheCallResponse
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Url { get; set; }
        public string Data { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class SearchRequest
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Term { get; set; }
        public int Type { get; set; }

        public string MediaTitle { get; set; }
        public string MediaDescription { get; set; }
        public string MediaUrlSmall { get; set; }
        public string MediaUrlMedium { get; set; }
        public string MediaUserName { get; set; }
        public string MediaUserAvatar { get; set; }
        public string EntityId { get; set; }
        public string MediaLicense { get; set; }
        public string ExtraText1 { get; set; }
        public string CacheCallResponseUrl { get; set; }

        public DateTime TimeStamp { get; set; }
    }


    public class GroupsRequest
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Term { get; set; }
        public int Type { get; set; }

        public string MediaTitle { get; set; }
        public string MediaDescription { get; set; }
        public string MediaUrlSmall { get; set; }
        public string MediaUrlMedium { get; set; }
        public string MediaUserName { get; set; }
        public string MediaUserAvatar { get; set; }
        public string EntityId { get; set; }
        public string MediaLicense { get; set; }
        public string ExtraText1 { get; set; }
        public string CacheCallResponseUrl { get; set; }
        public int PoolCount { get; set; }
        public int TopicCount { get; set; }
        public int MemberCount { get; set; }
        public bool EighteenPlus { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
