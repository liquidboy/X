/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2006-12-16
 * Time: 2:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace NChardet
{
	/// <summary>
	/// Description of PSMDetector.
	/// </summary>
	public abstract class PSMDetector
	{
		public static readonly int ALL                 =  0 ;
		public static readonly int JAPANESE            =  1 ;
		public static readonly int CHINESE             =  2 ;
		public static readonly int SIMPLIFIED_CHINESE  =  3 ;
		public static readonly int TRADITIONAL_CHINESE =  4 ;
		public static readonly int KOREAN              =  5 ;
		public static readonly int NO_OF_LANGUAGES     =  6 ;
		public static readonly int MAX_VERIFIERS       = 16 ;
		private Verifier[] mVerifier ;
		private EUCStatistics[] mStatisticsData ;
		private EUCSampler	mSampler = new EUCSampler() ;
		private byte[]    mState = new byte[MAX_VERIFIERS] ;
		private int[]     mItemIdx = new int[MAX_VERIFIERS] ;
		private int     mItems ;
		private int	   mClassItems ; 
		public bool mDone ;
		public bool mRunSampler ;
		public bool mClassRunSampler ;
		
		public PSMDetector()
		{
			initVerifiers( PSMDetector.ALL );
			Reset() ;
		}
		
		public PSMDetector(int langFlag) {
			initVerifiers(langFlag);
			Reset() ;
   		}
		
		public PSMDetector(int aItems, Verifier[] aVerifierSet,EUCStatistics[] aStatisticsSet)  {
			mClassRunSampler = ( aStatisticsSet != null ) ;
			mStatisticsData = aStatisticsSet ;
			mVerifier = aVerifierSet ;
			mClassItems = aItems ;
			Reset() ;
   		}
		
		public void Reset() {
			mRunSampler = mClassRunSampler ;
			mDone = false ;
			mItems = mClassItems ;
			for(int i=0; i<mItems; i++) {
			   mState[i] = 0;
			   mItemIdx[i] = i;
			}
			mSampler.Reset() ;
		}
				
		protected void initVerifiers(int currVerSet) {
			//int idx = 0 ;
        	int currVerifierSet ;

			if (currVerSet >=0 && currVerSet < NO_OF_LANGUAGES ) {
	   		currVerifierSet = currVerSet ;
			}
			else {
	   		currVerifierSet = PSMDetector.ALL ;
			}

			mVerifier = null ;
			mStatisticsData = null ;
		
			if ( currVerifierSet == PSMDetector.TRADITIONAL_CHINESE ) {
			   mVerifier = new Verifier[] {
		      		new UTF8Verifier(),
		      		new BIG5Verifier(),
		      		new ISO2022CNVerifier(),
		      		new EUCTWVerifier(),
		      		new CP1252Verifier(),
		      		new UCS2BEVerifier(),
		      		new UCS2LEVerifier()
			   };
				
			   mStatisticsData = new EUCStatistics[] {
		      		null,
		      		new Big5Statistics(),
		      		null,
		      		new EUCTWStatistics(),
		      		null,
		      		null,
		      		null
			   };
			}

			//==========================================================
			else if ( currVerifierSet == PSMDetector.KOREAN ) {
			   mVerifier = new Verifier[] {
		      		new UTF8Verifier(),
		      		new EUCKRVerifier(),
		      		new ISO2022KRVerifier(),
		      		new CP1252Verifier(),
		      		new UCS2BEVerifier(),
		      		new UCS2LEVerifier()
			   };
			}

			//==========================================================
			else if ( currVerifierSet == PSMDetector.SIMPLIFIED_CHINESE ) {
			   mVerifier = new Verifier[] {
		      		new UTF8Verifier(),
		      		new GB2312Verifier(),
		      		new GB18030Verifier(),
		      		new ISO2022CNVerifier(),
		      		new HZVerifier(),
		      		new CP1252Verifier(),
		      		new UCS2BEVerifier(),
		      		new UCS2LEVerifier()
			   };
			}

			//==========================================================
			else if ( currVerifierSet == PSMDetector.JAPANESE ) {
			   mVerifier = new Verifier[] {
		      		new UTF8Verifier(),
		      		new SJISVerifier(),
		      		new EUCJPVerifier(),
		      		new ISO2022JPVerifier(),
		      		new CP1252Verifier(),
		      		new UCS2BEVerifier(),
		      		new UCS2LEVerifier()
			   };
			}
			//==========================================================
			else if ( currVerifierSet == PSMDetector.CHINESE ) {
			   mVerifier = new Verifier[] {
		      		new UTF8Verifier(),
		      		new GB2312Verifier(),
		      		new GB18030Verifier(),
		      		new BIG5Verifier(),
		      		new ISO2022CNVerifier(),
		      		new HZVerifier(),
		      		new EUCTWVerifier(),
		      		new CP1252Verifier(),
		      		new UCS2BEVerifier(),
		      		new UCS2LEVerifier()
			   };
			   mStatisticsData = new EUCStatistics[] {
		      		null,
		      		new GB2312Statistics(),
				null,
		      		new Big5Statistics(),
		      		null,
		      		null,
		      		new EUCTWStatistics(),
		      		null,
		      		null,
		      		null
			   };
			}

			//==========================================================
			else if ( currVerifierSet == PSMDetector.ALL ) {
			   mVerifier = new Verifier[] {
		      		new UTF8Verifier(),
		      		new SJISVerifier(),
		      		new EUCJPVerifier(),
		      		new ISO2022JPVerifier(),
		      		new EUCKRVerifier(),
		      		new ISO2022KRVerifier(),
		      		new BIG5Verifier(),
		      		new EUCTWVerifier(),
		      		new GB2312Verifier(),
		      		new GB18030Verifier(),
		      		new ISO2022CNVerifier(),
		      		new HZVerifier(),
		      		new CP1252Verifier(),
		      		new UCS2BEVerifier(),
		      		new UCS2LEVerifier()
			   };
			   mStatisticsData = new EUCStatistics[] {
		      		null,
		      		null,
		      		new EUCJPStatistics(),
		      		null,
		      		new EUCKRStatistics(),
		      		null,
		      		new Big5Statistics(),
		      		new EUCTWStatistics(),
		      		new GB2312Statistics(),
		      		null,
		      		null,
		      		null,
		      		null,
		      		null,
		      		null
			   };
			}
			mClassRunSampler = ( mStatisticsData != null ) ;
		    mClassItems = mVerifier.Length ;
   		}
	  
   		public abstract void Report(String charset) ;
   		public bool HandleData(byte[] aBuf, int len) {
   			int i,j;
			byte b, st;
		 	for( i=0; i < len; i++) {
				b = aBuf[i] ;
				for (j=0; j < mItems; )
			   	{
				st = Verifier.getNextState( mVerifier[mItemIdx[j]], 
								b, mState[j]) ;
				if (st == Verifier.eItsMe) {
				   Report( mVerifier[mItemIdx[j]].charset() );
				   mDone = true ;
				   return mDone ;
			    } else if (st == Verifier.eError ) {
				   mItems--;
				   if (j < mItems ) {
					mItemIdx[j] = mItemIdx[mItems];	
					mState[j]   = mState[mItems];
				   }
				} else {
				    mState[j++] = st ;
				}
			 	}
				
		
			   if ( mItems <= 1 ) {
			        if( 1 == mItems) {
				   Report( mVerifier[mItemIdx[0]].charset() );
				}
				mDone = true ;
				return mDone ;
			   } 
			   else {
				
				int nonUCS2Num=0;
				int nonUCS2Idx=0;
		
				for(j=0; j<mItems; j++) {
				   if ( (!(mVerifier[mItemIdx[j]].isUCS2())) &&
					(!(mVerifier[mItemIdx[j]].isUCS2())) ) 
				   {
					nonUCS2Num++ ;
					nonUCS2Idx = j ;
				   }
				}

				if (1 == nonUCS2Num) {
				   Report( mVerifier[mItemIdx[nonUCS2Idx]].charset() );
				   mDone = true ;
				   return mDone ;
				}
			   }
				
		       } // End of for( i=0; i < len ...
			
		       if (mRunSampler)
		       	Sample(aBuf, len);
		       
		       return mDone ;
		   }
   
		   public void DataEnd() {
			if (mDone == true)
			    return ;
			if (mItems == 2) {
			    if ((mVerifier[mItemIdx[0]].charset()).Equals("GB18030")) {
				Report(mVerifier[mItemIdx[1]].charset()) ;
				mDone = true ;
			    } else if ((mVerifier[mItemIdx[1]].charset()).Equals("GB18030")) {
				Report(mVerifier[mItemIdx[0]].charset()) ;
				mDone = true ;
			    }
			}
			if (mRunSampler)
			   Sample(null, 0, true);
		   }
   		
		   public void Sample(byte[] aBuf, int aLen) {
			  Sample(aBuf, aLen, false) ;
		   }
		
		   public void Sample(byte[] aBuf, int aLen, bool aLastChance)
		   {
		      	int possibleCandidateNum  = 0;
			int j;
			int eucNum=0 ;
		
			for (j=0; j< mItems; j++) {
			   if (null != mStatisticsData[mItemIdx[j]]) 
				eucNum++ ;
			   if ((!mVerifier[mItemIdx[j]].isUCS2()) && 
					(!(mVerifier[mItemIdx[j]].charset()).Equals("GB18030")))
				possibleCandidateNum++ ;
			}

			mRunSampler = (eucNum > 1) ;
		     	if (mRunSampler) {
		            mRunSampler = mSampler.Sample(aBuf, aLen);
		            if(((aLastChance && mSampler.GetSomeData()) || 
		                mSampler.EnoughData())
		               && (eucNum == possibleCandidateNum)) {
		              mSampler.CalFreq();
		
		              int bestIdx = -1;
		              int eucCnt=0;
		              float bestScore = 0.0f;
		              for(j = 0; j < mItems; j++) {
		                 if((null != mStatisticsData[mItemIdx[j]])  &&
		                   (!(mVerifier[mItemIdx[j]].charset()).Equals("Big5")))
		                 {
		                    float score = mSampler.GetScore(
		                       mStatisticsData[mItemIdx[j]].mFirstByteFreq(),
		                       mStatisticsData[mItemIdx[j]].mFirstByteWeight(),
		                       mStatisticsData[mItemIdx[j]].mSecondByteFreq(),
		                       mStatisticsData[mItemIdx[j]].mSecondByteWeight() );

		                    if(( 0 == eucCnt++) || (bestScore > score )) {
		                       bestScore = score;
		                       bestIdx = j;
		                    } // if(( 0 == eucCnt++) || (bestScore > score )) 
		                } // if(null != ...)
		             } // for
		             if (bestIdx >= 0)
		             {
		               Report( mVerifier[mItemIdx[bestIdx]].charset());
		               mDone = true;
		             }
		           } // if (eucNum == possibleCandidateNum)
		         } // if(mRunSampler)
		   }
   
		   public String[] getProbableCharsets() {
		
			if (mItems <= 0) {
			   String[] nomatch = new String[1];
			   nomatch[0] = "nomatch" ;
			   return nomatch ;
			}
		
		   	string[] ret = new String[mItems] ;
			for (int i=0; i<mItems; i++)
				ret[i] = mVerifier[mItemIdx[i]].charset() ;
			return ret ;
		   }
	}
}
