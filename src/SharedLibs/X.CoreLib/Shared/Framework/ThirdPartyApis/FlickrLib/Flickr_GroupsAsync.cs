﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Browse the group category tree, finding groups and sub-categories.
        /// </summary>
        /// <remarks>
        /// Flickr no longer supports this method and it returns no useful information.
        /// </remarks>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<GroupCategory>> GroupsBrowseAsync()
        {
            return await GroupsBrowseAsync(null);
        }

        /// <summary>
        /// Browse the group category tree, finding groups and sub-categories.
        /// </summary>
        /// <param name="catId">The category id to fetch a list of groups and sub-categories for. If not specified, it defaults to zero, the root of the category tree.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<GroupCategory>> GroupsBrowseAsync(string catId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.browse");
            if (!String.IsNullOrEmpty(catId)) parameters.Add("cat_id", catId);

            return await GetResponseAsync<GroupCategory>(parameters);
        }

        /// <summary>
        /// Search the list of groups on Flickr for the text.
        /// </summary>
        /// <param name="text">The text to search for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<GroupSearchResultCollection>> GroupsSearchAsync(string text)
        {
            return await GroupsSearchAsync(text, 0, 0);
        }

        /// <summary>
        /// Search the list of groups on Flickr for the text.
        /// </summary>
        /// <param name="text">The text to search for.</param>
        /// <param name="page">The page of the results to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<GroupSearchResultCollection>> GroupsSearchAsync(string text, int page)
        {
            return await GroupsSearchAsync(text, page, 0);
        }

        /// <summary>
        /// Search the list of groups on Flickr for the text.
        /// </summary>
        /// <param name="text">The text to search for.</param>
        /// <param name="page">The page of the results to return.</param>
        /// <param name="perPage">The number of groups to list per page.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<GroupSearchResultCollection>> GroupsSearchAsync(string text, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.search");
            parameters.Add("api_key", apiKey);
            parameters.Add("text", text);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<GroupSearchResultCollection>(parameters);
        }

        /// <summary>
        /// Returns a <see cref="GroupFullInfo"/> object containing details about a group.
        /// </summary>
        /// <param name="groupId">The id of the group to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<GroupFullInfo>> GroupsGetInfoAsync(string groupId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.getInfo");
            parameters.Add("api_key", apiKey);
            parameters.Add("group_id", groupId);
            return await GetResponseAsync<GroupFullInfo>(parameters);
        }

        /// <summary>
        /// Get a list of group members.
        /// </summary>
        /// <param name="groupId">The group id to get the list of members for.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<MemberCollection>> GroupsMembersGetListAsync(string groupId)
        {
            return await GroupsMembersGetListAsync(groupId, 0, 0, MemberTypes.None);
        }

        /// <summary>
        /// Get a list of the members of a group. 
        /// </summary>
        /// <remarks>
        /// The call must be signed on behalf of a Flickr member, and the ability to see the group membership will be determined by the Flickr member's group privileges.
        /// </remarks>
        /// <param name="groupId">Return a list of members for this group. The group must be viewable by the Flickr member on whose behalf the API call is made.</param>
        /// <param name="page">The page of the results to return (default is 1).</param>
        /// <param name="perPage">The number of members to return per page (default is 100, max is 500).</param>
        /// <param name="memberTypes">The types of members to be returned. Can be more than one.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<MemberCollection>> GroupsMembersGetListAsync(string groupId, int page, int perPage, MemberTypes memberTypes)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.members.getList");
            parameters.Add("api_key", apiKey);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (memberTypes != MemberTypes.None) parameters.Add("membertypes", UtilityMethods.MemberTypeToString(memberTypes));
            parameters.Add("group_id", groupId);

            return await GetResponseAsync<MemberCollection>(parameters);
        }

        /// <summary>
        /// Adds a photo to a pool you have permission to add photos to.
        /// </summary>
        /// <param name="photoId">The id of one of your photos to be added.</param>
        /// <param name="groupId">The id of a group you are a member of.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> GroupsPoolsAddAsync(string photoId, string groupId)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.pools.add");
            parameters.Add("photo_id", photoId);
            parameters.Add("group_id", groupId);
            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Gets the context for a photo from within a group. This provides the
        /// id and thumbnail url for the next and previous photos in the group.
        /// </summary>
        /// <param name="photoId">The Photo ID for the photo you want the context for.</param>
        /// <param name="groupId">The group ID for the group you want the context to be relevant to.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Context>> GroupsPoolsGetContextAsync(string photoId, string groupId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.pools.getContext");
            parameters.Add("photo_id", photoId);
            parameters.Add("group_id", groupId);
            return await GetResponseAsync<Context>(parameters);
        }

        /// <summary>
        /// Remove a picture from a group.
        /// </summary>
        /// <param name="photoId">The id of one of your pictures you wish to remove.</param>
        /// <param name="groupId">The id of the group to remove the picture from.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> GroupsPoolsRemoveAsync(string photoId, string groupId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.pools.remove");
            parameters.Add("photo_id", photoId);
            parameters.Add("group_id", groupId);
            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Returns a list of groups to which you can add photos.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<MemberGroupInfoCollection>> GroupsPoolsGetGroupsAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.pools.getGroups");

            return await GetResponseAsync<MemberGroupInfoCollection>(parameters);
        }

        /// <summary>
        /// Gets a list of photos for a given group.
        /// </summary>
        /// <param name="groupId">The group ID for the group.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> GroupsPoolsGetPhotosAsync(string groupId)
        {
            return await GroupsPoolsGetPhotosAsync(groupId, null, null, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets a list of photos for a given group.
        /// </summary>
        /// <param name="groupId">The group ID for the group.</param>
        /// <param name="tags">Space seperated list of tags that photos returned must have.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> GroupsPoolsGetPhotosAsync(string groupId, string tags)
        {
            return await GroupsPoolsGetPhotosAsync(groupId, tags, null, PhotoSearchExtras.None, 0, 0);
        }

        /// <summary>
        /// Gets a list of photos for a given group.
        /// </summary>
        /// <param name="groupId">The group ID for the group.</param>
        /// <param name="perPage">The number of photos per page.</param>
        /// <param name="page">The page to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> GroupsPoolsGetPhotosAsync(string groupId, int page, int perPage)
        {
            return await GroupsPoolsGetPhotosAsync(groupId, null, null, PhotoSearchExtras.None, page, perPage);
        }

        /// <summary>
        /// Gets a list of photos for a given group.
        /// </summary>
        /// <param name="groupId">The group ID for the group.</param>
        /// <param name="tags">Space seperated list of tags that photos returned must have.</param>
        /// <param name="perPage">The number of photos per page.</param>
        /// <param name="page">The page to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> GroupsPoolsGetPhotosAsync(string groupId, string tags, int page, int perPage)
        {
            return await GroupsPoolsGetPhotosAsync(groupId, tags, null, PhotoSearchExtras.None, page, perPage);
        }

        /// <summary>
        /// Gets a list of photos for a given group.
        /// </summary>
        /// <param name="groupId">The group ID for the group.</param>
        /// <param name="tags">Space seperated list of tags that photos returned must have.
        /// Currently only supports 1 tag at a time.</param>
        /// <param name="userId">The group member to return photos for.</param>
        /// <param name="extras">The <see cref="PhotoSearchExtras"/> specifying which extras to return. All other overloads default to returning all extras.</param>
        /// <param name="perPage">The number of photos per page.</param>
        /// <param name="page">The page to return.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<PhotoCollection>> GroupsPoolsGetPhotosAsync(string groupId, string tags, string userId, PhotoSearchExtras extras, int page, int perPage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.groups.pools.getPhotos");
            parameters.Add("group_id", groupId);
            if (tags != null && tags.Length > 0) parameters.Add("tags", tags);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (userId != null && userId.Length > 0) parameters.Add("user_id", userId);
            if (extras != PhotoSearchExtras.None) parameters.Add("extras", UtilityMethods.ExtrasToString(extras));

            return await GetResponseAsync<PhotoCollection>(parameters);
        }
    }
}
