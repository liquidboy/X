using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Retrieve a temporary FROB from the Flickr service, to be used in redirecting the
        /// user to the Flickr web site for authentication. Only required for desktop authentication.
        /// </summary>
        /// <remarks>
        /// Pass the FROB to the <see cref="AuthCalcUrl"/> method to calculate the url.
        /// </remarks>
        /// <example>
        /// <code>
        /// string frob = flickr.AuthGetFrob();
        /// string url = flickr.AuthCalcUrl(frob, AuthLevel.Read);
        /// 
        /// // redirect the user to the url above and then wait till they have authenticated and return to the app.
        /// 
        /// Auth auth = flickr.AuthGetToken(frob);
        /// 
        /// // then store the auth.Token for later use.
        /// string token = auth.Token;
        /// </code>
        /// </example>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<string>> AuthGetFrobAsync()
        {
            CheckSigned();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.getFrob");

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            
            FlickrResult<string> result = new FlickrResult<string>();
            result.HasError = r.HasError;
            if (r.HasError)
            {
                result.Error = r.Error;
            }
            else
            {
                result.Result = r.Result.GetElementValue("frob");
            }
            return result;

            

        }

        /// <summary>
        /// After the user has authenticated your application on the flickr web site call this 
        /// method with the FROB (either stored from <see cref="AuthGetFrob"/> or returned in the URL
        /// from the Flickr web site) to get the users token.
        /// </summary>
        /// <param name="frob">The string containing the FROB.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Auth>> AuthGetTokenAsync(string frob)
        {
            CheckSigned();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.getToken");
            parameters.Add("frob", frob);

            var r = await GetResponseAsync<Auth>(parameters);
            if (!r.HasError)
            {
                AuthToken = r.Result.Token;
            }
            return r;
            
        }

        /// <summary>
        /// Gets the full token details for a given mini token, entered by the user following a 
        /// web based authentication.
        /// </summary>
        /// <param name="miniToken">The mini token.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Auth>> AuthGetFullTokenAsync(string miniToken)
        {
            CheckSigned();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.getFullToken");
            parameters.Add("mini_token", miniToken.Replace("-", String.Empty));

            var r = await GetResponseAsync<Auth>(parameters);
            if (!r.HasError)
            {
                AuthToken = r.Result.Token;
            }
            return r;
                
        }

        /// <summary>
        /// Checks the currently set authentication token with the flickr service to make
        /// sure it is still valid.
        /// </summary>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Auth>> AuthCheckTokenAsync()
        {
            return await AuthCheckTokenAsync(AuthToken);
        }

        /// <summary>
        /// Checks a authentication token with the flickr service to make
        /// sure it is still valid.
        /// </summary>
        /// <param name="token">The authentication token to check.</param>
        /// <param name="callback">Callback method to call upon return of the response from Flickr.</param>
        public async Task<FlickrResult<Auth>> AuthCheckTokenAsync(string token)
        {
            CheckSigned();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.checkToken");
            parameters.Add("auth_token", token);

            var r = await GetResponseAsync<Auth>(parameters);
            if (!r.HasError)
            {
                AuthToken = r.Result.Token;
            }
            return r;
        }

        /// <summary>
        /// Takes the currently (old) authentication Flickr instance and turns it OAuth authenticated instance.
        /// </summary>
        /// <remarks>
        /// Calling this method will also clear <see cref="Flickr.AuthToken"/> 
        /// and set <see cref="Flickr.OAuthAccessToken"/> and <see cref="Flickr.OAuthAccessTokenSecret"/>.
        /// </remarks>
        /// <param name="callback"></param>
        public async Task<FlickrResult<OAuthAccessToken>> AuthOAuthGetAccessTokenAsync()
        {
            CheckRequiresAuthentication();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.oauth.getAccessToken");

            var r = await GetResponseAsync<OAuthAccessToken>(parameters);
                
            if (!r.HasError)
            {
                OAuthAccessToken = r.Result.Token;
                OAuthAccessTokenSecret = r.Result.TokenSecret;

                AuthToken = null;
            }

            return r;

        }

    }
}
