﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Add a note to a picture.
        /// </summary>
        /// <param name="photoId">The photo id to add the note to.</param>
        /// <param name="noteX">The X co-ordinate of the upper left corner of the note.</param>
        /// <param name="noteY">The Y co-ordinate of the upper left corner of the note.</param>
        /// <param name="noteWidth">The width of the note.</param>
        /// <param name="noteHeight">The height of the note.</param>
        /// <param name="noteText">The text in the note.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> PhotosNotesAddAsync(string photoId, int noteX, int noteY, int noteWidth, int noteHeight, string noteText)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.notes.add");
            parameters.Add("photo_id", photoId);
            parameters.Add("note_x", noteX.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_y", noteY.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_w", noteWidth.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_h", noteHeight.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_text", noteText);

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            FlickrResult<string> result = new FlickrResult<string>();
            result.HasError = r.HasError;
            if (r.HasError)
            {
                result.Error = r.Error;
            }
            else
            {
                result.Result = r.Result.GetAttributeValue("*", "id");
            }
            return result;
            
        }

        /// <summary>
        /// Edit and update a note.
        /// </summary>
        /// <param name="noteId">The ID of the note to update.</param>
        /// <param name="noteX">The X co-ordinate of the upper left corner of the note.</param>
        /// <param name="noteY">The Y co-ordinate of the upper left corner of the note.</param>
        /// <param name="noteWidth">The width of the note.</param>
        /// <param name="noteHeight">The height of the note.</param>
        /// <param name="noteText">The new text in the note.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosNotesEditAsync(string noteId, int noteX, int noteY, int noteWidth, int noteHeight, string noteText)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.notes.edit");
            parameters.Add("note_id", noteId);
            parameters.Add("note_x", noteX.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_y", noteY.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_w", noteWidth.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_h", noteHeight.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            parameters.Add("note_text", noteText);

            return await GetResponseAsync<NoResponse>(parameters);
        }

        /// <summary>
        /// Delete an existing note.
        /// </summary>
        /// <param name="noteId">The ID of the note.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<NoResponse>> PhotosNotesDeleteAsync(string noteId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.photos.notes.delete");
            parameters.Add("note_id", noteId);

            return await GetResponseAsync<NoResponse>(parameters);
        }
    }
}
