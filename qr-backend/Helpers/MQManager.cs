using System;
using System.Collections;
using IBM.WMQ;
using qrbackend.Api.Services.BrokerHelper;

namespace qrbackend.Api.Helpers
{
    public class MQManager : IDisposable
    {
        #region Definiciones Globales.

        private MQQueueManager fMQueueManager = null;
        private MQQueue fQueue = null;
        private MQMessage fMQMsg = null;
        private MQPutMessageOptions fPutMsgOptions = null;
        private MQGetMessageOptions fGetMsgOptions = null;

        private string fQueueName = string.Empty;
        private string fQueueManagerName = string.Empty;
        private string fQueueIN = string.Empty;
        private string fQueueOUT = string.Empty;
        private string fChannelInfo = string.Empty;
        private string fQPort = string.Empty;
        private string fIp = string.Empty;
        private int fTimeOut = 0;

        private byte[] fCorrelationId = new byte[24];
        private byte[] fMessageId = new byte[24];
        private Hashtable fInitTabl = new Hashtable();

        private string fErrorTramaInEmpty = "La trama de entrada no puede estar en blanco.";

        private bool fIsConnected = false;

        #endregion

        public bool IsConnected
        {
            get { return fIsConnected; }
            set { fIsConnected = value; }
        }

        public MQManager(InfoBroker infoBroker)
        {
            fQueueManagerName = infoBroker.QueueManagerName;
            fChannelInfo = infoBroker.ChannelInfo;
            fIp = infoBroker.QIp;
            fQPort = infoBroker.QPort;
            fQueueIN = infoBroker.QueueIN;
            fQueueOUT = infoBroker.QueueOUT;
            fTimeOut = infoBroker.TimeOut;

            fInitTabl.Clear();

            if (fTimeOut == 0)
                fTimeOut = 60 * 1000;

            //  fInitTabl.Add(MQC.TRANSPORT_PROPERTY, connectionType);
            fInitTabl.Add(MQC.HOST_NAME_PROPERTY, fIp);
            fInitTabl.Add(MQC.PORT_PROPERTY, fQPort);
            fInitTabl.Add(MQC.CHANNEL_PROPERTY, fChannelInfo);
        }

        public void Connect()
        {
            try
            {
                if (fMQueueManager == null)
                    fMQueueManager = new MQQueueManager(fQueueManagerName, fInitTabl);

                if (!fMQueueManager.IsConnected)
                    fMQueueManager.Connect();

                fIsConnected = true;
            }
            catch (MQException mqe)
            {
                fIsConnected = false;
                throw new Exception(string.Format("Ha ocurrido un error intentando realizar la conexión con MQ. Error: {0}", mqe.Message));
            }
            catch (Exception ex)
            {
                ex = ex.GetBaseException();
                fIsConnected = false;
                throw new Exception(string.Format("Ha ocurrido un error intentando realizar la conexión con MQ. Error: {0}", ex.Message));
            }
        }

        public bool mtPutMessage(string varTrama)
        {
            try
            {
                if (string.IsNullOrEmpty(varTrama))
                    throw new Exception(fErrorTramaInEmpty);

                fQueue = fMQueueManager.AccessQueue(fQueueIN, MQC.MQOO_OUTPUT); //, fQueueManagerName, System.Guid.NewGuid().ToString(), fUser);

                fMQMsg = new MQMessage();
                fMQMsg.WriteBytes(varTrama);
                fMQMsg.Format = MQC.MQFMT_STRING;
                fMQMsg.MessageId = MQC.MQMI_NONE;
                fMQMsg.CorrelationId = MQC.MQCI_NONE;
                fMQMsg.CharacterSet = 819;
                fMQMsg.Encoding = 273;

                fPutMsgOptions = new MQPutMessageOptions();
                //fPutMsgOptions.Options = MQC.MQPMO_NEW_CORREL_ID;

                //coloca el mensaje en la trama
                fQueue.Put(fMQMsg, fPutMsgOptions);
                //fCorrelationId = fMQMsg.CorrelationId;
                fCorrelationId = fMQMsg.MessageId;

                return true;
            }
            catch (Exception ex)
            {
                if (ex is MQException)
                    throw new Exception(((MQException)ex).Message + " MQ Error Code: " + ((MQException)ex).ReasonCode.ToString());
                else
                    throw (ex);
            }
            finally
            {
                fQueue?.Close();
            }
        }

        public string mtGetMessage(int timeOut = 0)
        {
            string msgString = string.Empty;
            //bool isContinue = true;

            try
            {
                fQueue = fMQueueManager.AccessQueue(fQueueOUT, MQC.MQOO_INPUT_AS_Q_DEF + IBM.WMQ.MQC.MQOO_FAIL_IF_QUIESCING); //, fQueueManagerName, System.Guid.NewGuid().ToString(), fUser);

                fMQMsg = new MQMessage();
                fMQMsg.Format = MQC.MQFMT_STRING;

                fGetMsgOptions = new MQGetMessageOptions();
                fGetMsgOptions.WaitInterval = timeOut | fTimeOut;
                fGetMsgOptions.MatchOptions = MQC.MQMO_MATCH_CORREL_ID ;

                fMQMsg.MessageId = fCorrelationId;

                try
                {
                    fGetMsgOptions.MatchOptions = MQC.MQMO_MATCH_CORREL_ID;
                    fGetMsgOptions.Options = MQC.MQGMO_WAIT + MQC.MQGMO_ACCEPT_TRUNCATED_MSG;

                    //fMQMsg.MessageId = MQC.MQMI_NONE;
                    fMQMsg.CorrelationId = fCorrelationId;

                    fQueue.Get(fMQMsg, fGetMsgOptions);

                    msgString = fMQMsg.ReadString(fMQMsg.MessageLength);

                }
                catch (MQException ex)
                {
                    if(ex.Message.Contains("MQRC_NO_MSG_AVAILABLE"))
                        throw new Exception($"Time Out Broker: { ex.ReasonCode.ToString()} Descripción: {ex.Message}");

                    throw new Exception($"No message available. MQ Error Code: { ex.ReasonCode.ToString()} Descripción: {ex.Message}");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fQueue?.Close();
            }

            return msgString;
        }

        public void Dispose()
        {
            if (fMQueueManager.IsConnected)
            {
                fMQueueManager?.Disconnect();
                fMQueueManager = null;
            }
        }
    }
}
