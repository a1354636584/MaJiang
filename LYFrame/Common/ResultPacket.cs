using System;
using System.Runtime.Serialization;
/// <summary>
/// 通用返回类
/// </summary>
[Serializable]
[DataContract]
public class ResultPacket<T>
{
    
    public ResultPacket()
    {
    }

   
    /// <summary>
    /// 构造函数-请求成功
    /// </summary>
    public ResultPacket(T data, string message)
    {
        this.IsError = false;
        this.ResultCode = 0;
        this.Message = message;
        this.Data = data;
    }
  
    /// <summary>
    /// 构造函数-错误请求，状态码默认为error
    /// </summary>
    /// <param name="message">返回消息</param>
    public ResultPacket(string message)
    {
        this.IsError = true;
        this.ResultCode = 1;
        this.Message = message;
    }
    /// <summary>
    /// 构造函数-错误请求，需指定状态码
    /// </summary>
    /// <param name="resultCode">结果代码</param>
    /// <param name="message">返回消息</param>
    public ResultPacket(int resultCode, string message)
    {
        this.IsError = true;
        this.ResultCode = resultCode;
        this.Message = message;
    }
    /// <summary>
    /// 是否出错
    /// true：有错误1
    /// false：没有错误0
    /// </summary>
    [DataMember]
    public bool IsError
    {
        get;
        set;
    }
    /// <summary>
    /// 结果代码
    /// </summary>
    [DataMember]
    public int ResultCode
    {
        get;
        set;
    }
    /// <summary>
    /// 返回消息
    /// </summary>
    [DataMember]
    public string Message
    {
        get;
        set;
    }
    /// <summary>
    /// 响应数据
    /// </summary>
    [DataMember]
    public T Data
    {
        get;
        set;
    }
    /// <summary>
    /// 数据版本号
    /// </summary>
    [DataMember]
    public int? Version
    {
        get;
        set;
    }
   
}

//public enum ResultStatus : int
//{
//    /// <summary>
//    /// 成功
//    /// </summary>
//    [System.ComponentModel.Description("请求成功")]
//    Success = 0,
//    /// <summary>
//    /// 错误
//    /// </summary>
//    [System.ComponentModel.Description("请求错误")]
//    Error = 1,
//    /// <summary>
//    /// 未授权
//    /// </summary>
//    [System.ComponentModel.Description("请求未授权或者已超时")]
//    NotAuthor = 2,
//    /// <summary>
//    /// 异常
//    /// </summary>
//    [System.ComponentModel.Description("请求异常")]
//    Exception = 3,
//    /// <summary>
//    /// 输入验证未通过
//    /// </summary>
//    [System.ComponentModel.Description("输入项未通过")]
//    InputNotPassed = 4
//}