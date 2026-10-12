using Manisa.Web.Models;

namespace Manisa.Web.Services;

public static class LeadStatusHelper
{
    public static bool CanTransitionTo(LeadStatus current, LeadStatus next)
    {
        return (current, next) switch
        {
            (LeadStatus.New, LeadStatus.Contacted) => true,
            (LeadStatus.New, LeadStatus.Lost) => true,
            (LeadStatus.Contacted, LeadStatus.QuoteSent) => true,
            (LeadStatus.Contacted, LeadStatus.Lost) => true,
            (LeadStatus.QuoteSent, LeadStatus.Converted) => true,
            (LeadStatus.QuoteSent, LeadStatus.Lost) => true,
            (LeadStatus.Converted, LeadStatus.Converted) => true,
            (LeadStatus.Lost, LeadStatus.Lost) => true,
            _ => false
        };
    }
}
