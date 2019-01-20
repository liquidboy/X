/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2006-12-16
 * Time: 2:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace NChardet
{
	/// <summary>
	/// Description of ICharsetDetector.
	/// </summary>
	public interface ICharsetDetector
	{
		void Init(ICharsetDetectionObserver observer) ;
		bool DoIt(byte[] aBuf, int aLen, bool oDontFeedMe) ;
		void Done() ;
	}
}
