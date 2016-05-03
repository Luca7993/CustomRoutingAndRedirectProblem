using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomRoutingAndRedirectProblem.Framework
{
    public class MyRedirectResult : ActionResult
    {

        public MyRedirectResult(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IUrlHelper"/> for this result.
        /// </summary>


        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.HttpContext.Response.Redirect(Url);
        }

    }
}
