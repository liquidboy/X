﻿/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2006-12-16
 * Time: 3:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace NChardet
{
	/// <summary>
	/// Description of EUCKRStatistics.
	/// </summary>
	public class EUCKRStatistics : EUCStatistics
	{
		static float[] _mFirstByteFreq ;
     	static float   _mFirstByteStdDev;
     	static float   _mFirstByteMean;
     	static float   _mFirstByteWeight;
     	static float[] _mSecondByteFreq;
     	static float   _mSecondByteStdDev;
     	static float   _mSecondByteMean;
     	static float   _mSecondByteWeight;

     	public override float[] mFirstByteFreq() { return _mFirstByteFreq; }  
     	public override float   mFirstByteStdDev()  { return _mFirstByteStdDev; }  
     	public override float   mFirstByteMean()  { return _mFirstByteMean; }  
     	public override float   mFirstByteWeight()  { return _mFirstByteWeight; }  
     	public override float[] mSecondByteFreq()  { return _mSecondByteFreq; }  
     	public override float   mSecondByteStdDev()  { return _mSecondByteStdDev; }  
     	public override float   mSecondByteMean()  { return _mSecondByteMean; }  
     	public override float   mSecondByteWeight()  { return _mSecondByteWeight; }  

		public EUCKRStatistics() {

	    	_mFirstByteFreq = new float[] {
                  0.000000f, // FreqH[a1]
                  0.000000f, // FreqH[a2]
                  0.000000f, // FreqH[a3]
                  0.000000f, // FreqH[a4]
                  0.000000f, // FreqH[a5]
                  0.000000f, // FreqH[a6]
                  0.000000f, // FreqH[a7]
                  0.000412f, // FreqH[a8]
                  0.000000f, // FreqH[a9]
                  0.000000f, // FreqH[aa]
                  0.000000f, // FreqH[ab]
                  0.000000f, // FreqH[ac]
                  0.000000f, // FreqH[ad]
                  0.000000f, // FreqH[ae]
                  0.000000f, // FreqH[af]
                  0.057502f, // FreqH[b0]
                  0.033182f, // FreqH[b1]
                  0.002267f, // FreqH[b2]
                  0.016076f, // FreqH[b3]
                  0.014633f, // FreqH[b4]
                  0.032976f, // FreqH[b5]
                  0.004122f, // FreqH[b6]
                  0.011336f, // FreqH[b7]
                  0.058533f, // FreqH[b8]
                  0.024526f, // FreqH[b9]
                  0.025969f, // FreqH[ba]
                  0.054411f, // FreqH[bb]
                  0.019580f, // FreqH[bc]
                  0.063273f, // FreqH[bd]
                  0.113974f, // FreqH[be]
                  0.029885f, // FreqH[bf]
                  0.150041f, // FreqH[c0]
                  0.059151f, // FreqH[c1]
                  0.002679f, // FreqH[c2]
                  0.009893f, // FreqH[c3]
                  0.014839f, // FreqH[c4]
                  0.026381f, // FreqH[c5]
                  0.015045f, // FreqH[c6]
                  0.069456f, // FreqH[c7]
                  0.089860f, // FreqH[c8]
                  0.000000f, // FreqH[c9]
                  0.000000f, // FreqH[ca]
                  0.000000f, // FreqH[cb]
                  0.000000f, // FreqH[cc]
                  0.000000f, // FreqH[cd]
                  0.000000f, // FreqH[ce]
                  0.000000f, // FreqH[cf]
                  0.000000f, // FreqH[d0]
                  0.000000f, // FreqH[d1]
                  0.000000f, // FreqH[d2]
                  0.000000f, // FreqH[d3]
                  0.000000f, // FreqH[d4]
                  0.000000f, // FreqH[d5]
                  0.000000f, // FreqH[d6]
                  0.000000f, // FreqH[d7]
                  0.000000f, // FreqH[d8]
                  0.000000f, // FreqH[d9]
                  0.000000f, // FreqH[da]
                  0.000000f, // FreqH[db]
                  0.000000f, // FreqH[dc]
                  0.000000f, // FreqH[dd]
                  0.000000f, // FreqH[de]
                  0.000000f, // FreqH[df]
                  0.000000f, // FreqH[e0]
                  0.000000f, // FreqH[e1]
                  0.000000f, // FreqH[e2]
                  0.000000f, // FreqH[e3]
                  0.000000f, // FreqH[e4]
                  0.000000f, // FreqH[e5]
                  0.000000f, // FreqH[e6]
                  0.000000f, // FreqH[e7]
                  0.000000f, // FreqH[e8]
                  0.000000f, // FreqH[e9]
                  0.000000f, // FreqH[ea]
                  0.000000f, // FreqH[eb]
                  0.000000f, // FreqH[ec]
                  0.000000f, // FreqH[ed]
                  0.000000f, // FreqH[ee]
                  0.000000f, // FreqH[ef]
                  0.000000f, // FreqH[f0]
                  0.000000f, // FreqH[f1]
                  0.000000f, // FreqH[f2]
                  0.000000f, // FreqH[f3]
                  0.000000f, // FreqH[f4]
                  0.000000f, // FreqH[f5]
                  0.000000f, // FreqH[f6]
                  0.000000f, // FreqH[f7]
                  0.000000f, // FreqH[f8]
                  0.000000f, // FreqH[f9]
                  0.000000f, // FreqH[fa]
                  0.000000f, // FreqH[fb]
                  0.000000f, // FreqH[fc]
                  0.000000f, // FreqH[fd]
                  0.000000f  // FreqH[fe]

		};

		_mFirstByteStdDev = 0.025593f; // Lead Byte StdDev
		_mFirstByteMean   = 0.010638f; // Lead Byte Mean
		_mFirstByteWeight = 0.647437f; // Lead Byte Weight

		_mSecondByteFreq = new float[] {
                  0.016694f, // FreqL[a1]
                  0.000000f, // FreqL[a2]
                  0.012778f, // FreqL[a3]
                  0.030091f, // FreqL[a4]
                  0.002679f, // FreqL[a5]
                  0.006595f, // FreqL[a6]
                  0.001855f, // FreqL[a7]
                  0.000824f, // FreqL[a8]
                  0.005977f, // FreqL[a9]
                  0.004740f, // FreqL[aa]
                  0.003092f, // FreqL[ab]
                  0.000824f, // FreqL[ac]
                  0.019580f, // FreqL[ad]
                  0.037304f, // FreqL[ae]
                  0.008244f, // FreqL[af]
                  0.014633f, // FreqL[b0]
                  0.001031f, // FreqL[b1]
                  0.000000f, // FreqL[b2]
                  0.003298f, // FreqL[b3]
                  0.002061f, // FreqL[b4]
                  0.006183f, // FreqL[b5]
                  0.005977f, // FreqL[b6]
                  0.000824f, // FreqL[b7]
                  0.021847f, // FreqL[b8]
                  0.014839f, // FreqL[b9]
                  0.052968f, // FreqL[ba]
                  0.017312f, // FreqL[bb]
                  0.007626f, // FreqL[bc]
                  0.000412f, // FreqL[bd]
                  0.000824f, // FreqL[be]
                  0.011129f, // FreqL[bf]
                  0.000000f, // FreqL[c0]
                  0.000412f, // FreqL[c1]
                  0.001649f, // FreqL[c2]
                  0.005977f, // FreqL[c3]
                  0.065746f, // FreqL[c4]
                  0.020198f, // FreqL[c5]
                  0.021434f, // FreqL[c6]
                  0.014633f, // FreqL[c7]
                  0.004122f, // FreqL[c8]
                  0.001649f, // FreqL[c9]
                  0.000824f, // FreqL[ca]
                  0.000824f, // FreqL[cb]
                  0.051937f, // FreqL[cc]
                  0.019580f, // FreqL[cd]
                  0.023289f, // FreqL[ce]
                  0.026381f, // FreqL[cf]
                  0.040396f, // FreqL[d0]
                  0.009068f, // FreqL[d1]
                  0.001443f, // FreqL[d2]
                  0.003710f, // FreqL[d3]
                  0.007420f, // FreqL[d4]
                  0.001443f, // FreqL[d5]
                  0.013190f, // FreqL[d6]
                  0.002885f, // FreqL[d7]
                  0.000412f, // FreqL[d8]
                  0.003298f, // FreqL[d9]
                  0.025969f, // FreqL[da]
                  0.000412f, // FreqL[db]
                  0.000412f, // FreqL[dc]
                  0.006183f, // FreqL[dd]
                  0.003298f, // FreqL[de]
                  0.066983f, // FreqL[df]
                  0.002679f, // FreqL[e0]
                  0.002267f, // FreqL[e1]
                  0.011129f, // FreqL[e2]
                  0.000412f, // FreqL[e3]
                  0.010099f, // FreqL[e4]
                  0.015251f, // FreqL[e5]
                  0.007626f, // FreqL[e6]
                  0.043899f, // FreqL[e7]
                  0.003710f, // FreqL[e8]
                  0.002679f, // FreqL[e9]
                  0.001443f, // FreqL[ea]
                  0.010923f, // FreqL[eb]
                  0.002885f, // FreqL[ec]
                  0.009068f, // FreqL[ed]
                  0.019992f, // FreqL[ee]
                  0.000412f, // FreqL[ef]
                  0.008450f, // FreqL[f0]
                  0.005153f, // FreqL[f1]
                  0.000000f, // FreqL[f2]
                  0.010099f, // FreqL[f3]
                  0.000000f, // FreqL[f4]
                  0.001649f, // FreqL[f5]
                  0.012160f, // FreqL[f6]
                  0.011542f, // FreqL[f7]
                  0.006595f, // FreqL[f8]
                  0.001855f, // FreqL[f9]
                  0.010923f, // FreqL[fa]
                  0.000412f, // FreqL[fb]
                  0.023702f, // FreqL[fc]
                  0.003710f, // FreqL[fd]
                  0.001855f  // FreqL[fe]

		};

		_mSecondByteStdDev = 0.013937f; // Trail Byte StdDev
		_mSecondByteMean   = 0.010638f; // Trail Byte Mean
		_mSecondByteWeight = 0.352563f; // Trial Byte Weight
		}

	}
}
