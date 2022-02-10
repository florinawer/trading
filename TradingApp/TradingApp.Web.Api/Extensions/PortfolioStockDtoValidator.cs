using FluentValidation;
using TradingApp.Web.Dto;

namespace TradingApp.Web.Api.Extensions
{
    public class PortfolioStockDtoValidator : AbstractValidator<PortfolioStockDTO>
    {
        public PortfolioStockDtoValidator()
        {
            RuleFor(model => model.Amount).NotEmpty();
        }
    }
}
