using Accounts;
using Events;
using Orders;
using Performances;
using ServerLogin;
using System;
using System.Security.AccessControl;
using Ticket;

namespace WSDL
{
    class Program
    {
        static void Main(string[] args)
        {

            /************************************Login***************************************************/
            WsLoginClient bOSLogin2 = new WsLoginClient();

            UserLogin2Request loginRequest = new UserLogin2Request
            {

                AWorkstationAK = "VTA.WKS19",
                AUserName = "WebUser-01",
                APassword = "BOSweb123"

            };
            
            UserLogin2Response response = bOSLogin2.UserLogin2(loginRequest);

            string login_response = response.@return;

            Console.WriteLine(login_response);


            goto Event;
            //goto Performance;
            //goto Save;
            //goto B2B;
            //goto Checkout;
            //goto Close;
            //goto Print;
            //goto Logout;

        /************************************Event***************************************************/

          Event:
            
            WsAPIEventClient bosEvent = new WsAPIEventClient();

            FindAllEventsRequest eventsRequest = new FindAllEventsRequest();
            FindAllEventsResponse eventsResponse = bosEvent.FindAllEvents(eventsRequest);

            FINDALLEVENTSRESP fINDALLEVENTSRESP = eventsResponse.@return;

            string event_response = fINDALLEVENTSRESP.EVENTLIST.ToString();



            /************************************Performance***************************************************/

           Performance:

            WsAPIPerformanceClient wsAPIPerformanceClient = new WsAPIPerformanceClient();

            SEARCHPERFORMANCEREQ sEARCHPERFORMANCEREQ = new SEARCHPERFORMANCEREQ();
            sEARCHPERFORMANCEREQ.EVENTAK = "VTA.EVN3";
            sEARCHPERFORMANCEREQ.SELLABLE = true;
            SearchPerformanceRequest searchPerformanceRequest = new SearchPerformanceRequest(sEARCHPERFORMANCEREQ);

            SearchPerformanceResponse searchPerformanceResponse = wsAPIPerformanceClient.SearchPerformance(searchPerformanceRequest);

            SEARCHPERFORMANCERESP sEARCHPERFORMANCERESP = searchPerformanceResponse.@return;

            string performance_response = sEARCHPERFORMANCERESP.PERFORMANCELIST.ToString();


            /************************************Save account***************************************************/

            Save:
            WsAPIAccountClient wsAPIAccountClient = new WsAPIAccountClient();


            Accounts.FIELDLISTFIELD[] fIELDLISTFIELDs = new Accounts.FIELDLISTFIELD[3];

            Accounts.FIELDLISTFIELD fIELDLISTFIELD1 = new Accounts.FIELDLISTFIELD();
            fIELDLISTFIELD1.OBJTYPE = 1;
            fIELDLISTFIELD1.VALUE = "ironman";//firstname

            Accounts.FIELDLISTFIELD fIELDLISTFIELD2 = new Accounts.FIELDLISTFIELD();
            fIELDLISTFIELD2.OBJTYPE = 548;
            fIELDLISTFIELD2.VALUE = "tony1234";//firstname


            Accounts.FIELDLISTFIELD fIELDLISTFIELD3 = new Accounts.FIELDLISTFIELD();
            fIELDLISTFIELD3.OBJTYPE = 549;
            fIELDLISTFIELD3.VALUE = "avengers";//firstname



            fIELDLISTFIELDs[0] = fIELDLISTFIELD1;
            fIELDLISTFIELDs[1] = fIELDLISTFIELD2;
            fIELDLISTFIELDs[2] = fIELDLISTFIELD3;

            SAVEACCOUNTREQ sAVEACCOUNTREQ = new SAVEACCOUNTREQ
            {
                FIELDLIST = fIELDLISTFIELDs,
                DMGCATEGORYAK = "VTA.DMGCAT29"

            };
            SaveAccountRequest saveAccountRequest = new SaveAccountRequest(sAVEACCOUNTREQ);

            SaveAccountResponse saveAccountResponse = wsAPIAccountClient.SaveAccount(saveAccountRequest);

            SAVEACCOUNTRESP sAVEACCOUNTRESP = saveAccountResponse.@return;

            string save_response = sAVEACCOUNTRESP.BASICINFO.ACCOUNTAK;

            /************************************B2B Login***************************************************/


            B2B:
            B2BAccountLogInRequest b2BAccountLogInRequest = new B2BAccountLogInRequest
            {
                ADmgCatCode = "Guests",
                AUsername = "batman",
                APsw = "Iambatma"
            };

            B2BAccountLogInResponse b2BAccountLogInResponse = wsAPIAccountClient.B2BAccountLogIn(b2BAccountLogInRequest);

            /************************************Checkout***************************************************/
            
            Checkout:
            WsAPIOrderClient wsAPIOrderClient = new WsAPIOrderClient();


            CHECKOUTREQ cHECKOUTREQ = new CHECKOUTREQ();


            ITEMLISTITEM[] iTEMLISTITEMs = new ITEMLISTITEM[1];

            ITEMLISTITEMPERFORMANCE[] iTEMLISTITEMPERFORMANCEs = new ITEMLISTITEMPERFORMANCE[1];

            iTEMLISTITEMPERFORMANCEs[1].AK = "VTA.EVN4.PRF100";

            iTEMLISTITEMs[1].AK = "VTA.EVN1.MCC28";
            iTEMLISTITEMs[1].QTY = "1";
            iTEMLISTITEMs[1].PERFORMANCELIST = iTEMLISTITEMPERFORMANCEs;


            Orders.ACCOUNTSAVEBASE aCCOUNTSAVEBASE = new Orders.ACCOUNTSAVEBASE
            {
                AK = "99901920000019"
            };

            ORDERSTATUS oRDERSTATUS = new ORDERSTATUS
            {
                APPROVED = true,
                PAID = true,
                ENCODED = true,
                VALIDATED = true,
                COMPLETED = true
            };


            RESERVATIONBASE rESERVATIONBASE = new RESERVATIONBASE
            {

                RESERVATIONOWNER = aCCOUNTSAVEBASE
            };

            cHECKOUTREQ.SHOPCART = new SHOPCART
            {
                ITEMLIST = iTEMLISTITEMs,
                RESERVATION = rESERVATIONBASE,
                FLAG = oRDERSTATUS
            };


            CheckOutRequest checkOutRequest = new CheckOutRequest(cHECKOUTREQ);

            CheckOutResponse checkOutResponse = wsAPIOrderClient.CheckOut(checkOutRequest);

            CHECKOUTRESP cHECKOUTRESP = checkOutResponse.@return;

            string checkout_response_sale_ak = cHECKOUTRESP.SALE.AK;
            float checkout_response_sale_gross = cHECKOUTRESP.SALE.TOTAL.GROSS;




            /************************************Close_order***************************************************/


            Close:
            Orders.PAYMENTLISTBASEPAYMENTINFO[] pAYMENTLISTBASEPAYMENTINFOs = new Orders.PAYMENTLISTBASEPAYMENTINFO[1];

            pAYMENTLISTBASEPAYMENTINFOs[1].CODE = "";
            pAYMENTLISTBASEPAYMENTINFOs[1].AMOUNT = checkout_response_sale_gross;

          



            CLOSEORDERREQ cLOSEORDERREQ = new CLOSEORDERREQ {
                AK = checkout_response_sale_ak,
                PAYMENTINFOLIST = pAYMENTLISTBASEPAYMENTINFOs,
               

            };

            CloseOrderRequest closeOrderRequest = new CloseOrderRequest(cLOSEORDERREQ);
            CloseOrderResponse closeOrderResponse = wsAPIOrderClient.CloseOrder(closeOrderRequest);

            CLOSEORDERRESP cLOSEORDERRESP = closeOrderResponse.@return;

            string close_order_sale_ak = cLOSEORDERRESP.SALE.AK;


            /************************************Print Ticket***************************************************/

            Print:
            WsAPITicketClient wsAPITicketClient = new WsAPITicketClient();


            
            PrintPdfTicketRequest printPdfTicketRequest = new PrintPdfTicketRequest { 
            ASaleAk = "VTA.WKS19.200527.SAL1" //VTA.WKS19.200527.SAL1
            };

            PrintPdfTicketResponse printPdfTicketResponse = wsAPITicketClient.PrintPdfTicket(printPdfTicketRequest);

            PRINTPDFTICKETRESP pRINTPDFTICKETRESP = printPdfTicketResponse.@return;

            byte[] pdf_string = pRINTPDFTICKETRESP.PDF;


            /************************************Logout***************************************************/

            Logout:
            UserLogoutRequest userLogoutRequest = new UserLogoutRequest();

            UserLogoutResponse userLogoutResponse = bOSLogin2.UserLogout(userLogoutRequest);





        }
    }
}
