using System;

namespace WorldDomination.Web.Mvc
{
    public interface IErrorViewModel
    {
        Exception Exception { get; set; }
    }
}