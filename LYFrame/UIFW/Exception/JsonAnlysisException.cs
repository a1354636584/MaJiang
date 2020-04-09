/***  
 *      Json 解析异常
 *      专门负责对于JSon 由于路径错误，或者Json 格式错误造成的异常，进行捕获。
 *   
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LYFrame
{
	public class JsonAnlysisException : Exception {
	    public JsonAnlysisException() : base(){}
	    public JsonAnlysisException(string exceptionMessage) : base(exceptionMessage){}
	}
}