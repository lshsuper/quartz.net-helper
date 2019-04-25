using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dm.Model.V20151123;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace quartz.net.helper.task
{
    /// <summary>
    /// 调度任务
    /// </summary>
    [DisallowConcurrentExecution]
    public class SayHelloJob : IJob
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public Task Execute(IJobExecutionContext context)
        {

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "LTAINKJGWLmATlqd", "X7hVz2i0MZfh1SpUtERomAUXfzLudf");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendMailRequest request = new SingleSendMailRequest();
            try
            {
                //Version must set to "2017-06-22" when the regionId is not "cn-hangzhou"
                //request.Version = "2017-06-22";
                request.AccountName = "lshsuper@api.lshblog.club";
                request.FromAlias = "lsh";
                request.AddressType = 1;
                request.TagName = "控制台创建的标签";
                request.ReplyToAddress = true;
                request.ToAddress = "1308956271@qq.com";
                request.Subject = "默认标题";
                request.HtmlBody = "默认主题"+ context.Trigger.JobKey;

                SingleSendMailResponse httpResponse = client.GetAcsResponse(request);

            }
            catch (System.Runtime.Remoting.ServerException e)
            {

            }
            catch (ClientException e)
            {

            }
            catch (Exception e)
            {

            }
            Console.WriteLine(context.Trigger.JobKey);
            return Task.FromResult(0);
        }
    }
}
