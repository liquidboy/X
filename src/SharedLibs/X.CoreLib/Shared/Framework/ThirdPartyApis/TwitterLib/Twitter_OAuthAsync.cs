using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Twitter
    {
        /// <summary>
        /// Get an <see cref="OAuthRequestToken"/> for the given callback URL.
        /// </summary>
        /// <remarks>Specify 'oob' as the callback url for no callback to be performed.</remarks>
        /// <param name="callbackUrl">The callback Uri, or 'oob' if no callback is to be performed.</param>
        /// <param name="callback"></param>
        public async Task<FlickrResult<OAuthRequestToken>> OAuthGetRequestTokenAsync(string callbackUrl)
        {

            string url = RequestTokenUrl; //"https://www.flickr.com/services/oauth/request_token";

            Dictionary<string, string> parameters = OAuthGetBasicParameters();

            parameters.Add("oauth_callback", callbackUrl);

            string sig = OAuthCalculateSignature("POST", url, parameters, null);

            parameters.Add("oauth_signature", sig);

            FlickrResult<string> r = await TwitterResponder.GetDataResponseAsync(this, "POST", url, parameters);

            FlickrResult<OAuthRequestToken> result = new FlickrResult<OAuthRequestToken>();

            if (!r.HasError)
            {
                result.Result = FlickrNet.OAuthRequestToken.ParseResponse(r.Result);
            }
            else
            {
                result.HasError = r.HasError;
                result.Error = r.Error;
                result.ErrorCode = r.ErrorCode;
                result.ErrorMessage = r.ErrorMessage;
            }

            return result;

        }

        /// <summary>
        /// Returns an access token for the given request token, secret and authorization verifier.
        /// </summary>
        /// <param name="requestToken"></param>
        /// <param name="verifier"></param>
        /// <param name="callback"></param>
        public async Task<FlickrResult<OAuthAccessToken>> OAuthGetAccessTokenAsync(OAuthRequestToken requestToken, string verifier)
        {
            return await OAuthGetAccessTokenAsync(requestToken.Token, requestToken.TokenSecret, verifier);
        }

        /// <summary>
        /// For a given request token and verifier string return an access token.
        /// </summary>
        /// <param name="requestToken"></param>
        /// <param name="requestTokenSecret"></param>
        /// <param name="verifier"></param>
        /// <param name="callback"></param>
        public async Task<FlickrResult<OAuthAccessToken>> OAuthGetAccessTokenAsync(string requestToken, string requestTokenSecret, string verifier)
        {
            string url = "https://api.twitter.com/oauth/access_token";

            Dictionary<string, string> parameters = OAuthGetBasicParameters();

            parameters.Add("oauth_verifier", verifier);
            parameters.Add("oauth_token", requestToken);

            string sig = OAuthCalculateSignature("POST", url, parameters, requestTokenSecret);

            parameters.Add("oauth_signature", sig);

            FlickrResult<string> r = await TwitterResponder.GetDataResponseAsync(this, "POST", url, parameters);

            FlickrResult<OAuthAccessToken> result = new FlickrResult<OAuthAccessToken>();

            if (!r.HasError)
            {
                result.Result = FlickrNet.OAuthAccessToken.ParseResponse(r.Result);
            }
            else
            {
                result.HasError = r.HasError;
                result.Error = r.Error;
                result.ErrorCode = r.ErrorCode;
                result.ErrorMessage = r.ErrorMessage;
            }

            return result;

        }


    }
}

