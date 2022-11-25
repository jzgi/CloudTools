﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TencentSDK.Pay
{
    /// <summary>
    /// 发起商家转账API 请求数据
    /// </summary>
    public class BatchesRequestData
    {
        public BatchesRequestData(string appid, string out_batch_no, string batch_name, string batch_remark, int total_amount, int total_num, Transfer_Detail_List[] transfer_detail_list)
        {
            this.appid = appid;
            this.out_batch_no = out_batch_no;
            this.batch_name = batch_name;
            this.batch_remark = batch_remark;
            this.total_amount = total_amount;
            this.total_num = total_num;
            this.transfer_detail_list = transfer_detail_list;
        }

        /// <summary>
        /// 直连商户的appid	
        /// <para>申请商户号的appid或商户号绑定的appid（企业号corpid即为此appid）。示例值：wxf636efh567hg4356</para>
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商家批次单号
        /// <para>商户系统内部的商家批次单号，要求此参数只能由数字、大小写字母组成，在商户系统内部唯一。示例值：plfk2020042013</para>
        /// </summary>
        public string out_batch_no { get; set; }
        /// <summary>
        /// 批次名称
        /// <para>商户系统内部的商家批次单号，要求此参数只能由数字、大小写字母组成，在商户系统内部唯一。示例值：plfk2020042013</para>
        /// </summary>
        public string batch_name { get; set; }
        /// <summary>
        /// 批次备注
        /// <para>转账说明，UTF8编码，最多允许32个字符。示例值：2019年1月深圳分部报销单</para>
        /// </summary>
        public string batch_remark { get; set; }
        /// <summary>
        /// 转账总金额
        /// <para>转账金额单位为“分”。转账总金额必须与批次内所有明细转账金额之和保持一致，否则无法发起转账操作。示例值：4000000</para>
        /// </summary>
        public int total_amount { get; set; }
        /// <summary>
        /// 转账总笔数
        /// <para>一个转账批次单最多发起三千笔转账。转账总笔数必须与批次内所有明细之和保持一致，否则无法发起转账操作。示例值：200</para>
        /// </summary>
        public int total_num { get; set; }
        /// <summary>
        /// 转账明细列表
        /// <para>发起批量转账的明细列表，最多三千笔</para>
        /// </summary>
        public Transfer_Detail_List[] transfer_detail_list { get; set; }
    }

    public class Transfer_Detail_List
    {
        /// <summary>
        /// 商家明细单号
        /// <para>商户系统内部区分转账批次单下不同转账明细单的唯一标识，要求此参数只能由数字、大小写字母组成。示例值：x23zy545Bd5436</para>
        /// </summary>
        public string out_detail_no { get; set; }
        /// <summary>
        /// 转账金额
        /// <para>转账金额单位为分。示例值：200000</para>
        /// </summary>
        public int transfer_amount { get; set; }
        /// <summary>
        /// 转账备注
        /// <para>单条转账备注（微信用户会收到该备注），UTF8编码，最多允许32个字符。示例值：2020年4月报销</para>
        /// </summary>
        public string transfer_remark { get; set; }
        /// <summary>
        /// 用户在直连商户应用下的用户标示	
        /// <para>openid是微信用户在公众号appid下的唯一用户标识（appid不同，则获取到的openid就不同），可用于永久标记一个用户。</para>
        /// <para><see href="https://pay.weixin.qq.com/wiki/doc/apiv3/terms_definition/chapter1_1_3.shtml">获取openid</see></para>
        /// <para>示例值：o-MYE42l80oelYMDE34nYD456Xoy</para>
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 收款用户姓名
        /// <para>1、明细转账金额 >= 2,000，收款用户姓名必填；</para>
        /// <para>2、同一批次转账明细中，收款用户姓名字段需全部填写、或全部不填写；</para>
        /// <para>3、 若传入收款用户姓名，微信支付会校验用户openID与姓名是否一致，并提供电子回单；</para>
        /// <para>4、收款方姓名。采用标准RSA算法，公钥由微信侧提供</para>
        /// <para>5、该字段需进行加密处理，加密方法详见敏感信息加密说明。(提醒：必须在HTTP头中上送Wechatpay-Serial)</para>
        /// <para>6、商户需确保收集用户的姓名信息，以及向微信支付传输用户姓名和账号标识信息做一致性校验已合法征得用户授权</para>
        /// <para>示例值：757b340b45ebef5467rter35gf464344v3542sdf4t6re4tb4f54ty45t4yyry45</para>
        /// </summary>
        public string user_name { get; set; }
    }

}
