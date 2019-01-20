/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2006-12-16
 * Time: 3:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace NChardet
{
	/// <summary>
	/// Description of Detector.
	/// </summary>
	public class Detector : PSMDetector,ICharsetDetector
	{
		ICharsetDetectionObserver mObserver = null ;
		
		public Detector():base() 
        {
		}
	
		public Detector(int langFlag):base(langFlag) 
        {
		}
	
		public void Init(ICharsetDetectionObserver aObserver) {
		  	mObserver = aObserver ;
			return ;
		}

		public bool DoIt(byte[] aBuf, int aLen, bool oDontFeedMe) {
			if (aBuf == null || oDontFeedMe )
			    return false ;
	
			this.HandleData(aBuf, aLen) ;	
			return mDone ;
		}
	
		public void Done() {
			this.DataEnd() ;
			return ;
		}

		public override void Report(string charset) {
			if (mObserver != null)
			    mObserver.Notify(charset)  ;
		}
	
		public bool isAscii(byte[] aBuf, int aLen) {
	                for(int i=0; i<aLen; i++) {
	                   if ((0x0080 & aBuf[i]) != 0) {
	                      return false ;
	                   }
	                }
			return true ;
		}
	}
}
