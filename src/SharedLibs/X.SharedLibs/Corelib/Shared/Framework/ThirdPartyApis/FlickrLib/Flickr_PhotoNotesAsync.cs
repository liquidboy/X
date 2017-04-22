using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        
        public async Task<FlickrResult<PhotoNoteCollection>> PhotoNotesGetListAsync(string photoId)
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.notes.getList");
            parameters.Add("photo_id", photoId);
            return await GetResponseAsync<PhotoNoteCollection>(parameters);
        }


    }
}
