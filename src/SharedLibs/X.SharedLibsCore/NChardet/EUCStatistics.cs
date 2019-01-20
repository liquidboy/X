/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2006-12-16
 * Time: 2:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace NChardet
{
	/// <summary>
	/// Description of EUCStatistics.
	/// </summary>
	public abstract class EUCStatistics
	{
		public abstract float[] mFirstByteFreq() ;
     	public abstract float   mFirstByteStdDev();
     	public abstract float   mFirstByteMean();
     	public abstract float   mFirstByteWeight();
     	public abstract float[] mSecondByteFreq();
     	public abstract float   mSecondByteStdDev();
     	public abstract float   mSecondByteMean();
     	public abstract float   mSecondByteWeight();
     	
		public EUCStatistics()
		{
		}
	}
}
