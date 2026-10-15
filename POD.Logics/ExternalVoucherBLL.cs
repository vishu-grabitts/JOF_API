using POD.DAL;
using POD.Models;

namespace POD.Logics
{
    public class ExternalVoucherBLL
    {
        private readonly ExternalVoucherDAL _dal;

        public ExternalVoucherBLL()
        {
            _dal = new ExternalVoucherDAL();
        }

        public VoucherResponse SubmitSalesOrder(SalesOrderRequest request)
        {
            VoucherResponse response = new VoucherResponse();

            if (request == null)
            {
                response.Success = false;
                response.Message =
                    "Request cannot be null.";

                return response;
            }

            if (string.IsNullOrWhiteSpace(request.VoucherNo))
            {
                response.Success = false;
                response.Message = "VoucherNo is required.";
                return response;
            }

            if (request.VoucherLines == null ||
                request.VoucherLines.Count == 0)
            {
                response.Success = false;
                response.Message =
                    "At least one voucher line is required.";

                return response;
            }
           

            bool inserted =_dal.InsertSalesOrder(request);

            if (inserted)
            {
                response.Success = true;
                response.Message =
                    "Voucher submitted successfully.";

                response.VoucherNo =
                    request.VoucherNo;
            }
            else
            {
                response.Success = false;
                response.Message =
                    "Voucher could not be submitted.";

                response.VoucherNo =
                    request.VoucherNo;
            }

            return response;
        }
    }
}