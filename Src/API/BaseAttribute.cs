using System;
using System.Collections.Generic;
using System.Linq;
using ApiDebts.Src.DAO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDebts.Src.API
{
    public abstract class BaseAttribute : Attribute, IAuthorizationFilter
    {
        public DebtsContext Context { get; }

        public BaseAttribute(DebtsContext context) : base()
        {
            Context = context;
        }

        public void Error(string mensaje)
        {
            throw new Exception(mensaje);
        }

        protected int GetHeaderInt(AuthorizationFilterContext context, string name)
        {
            var rq = context.HttpContext.Request;
            var headers = rq.Headers;
            if (!headers.ContainsKey(name))
            {
                Error($"Debe mandar {name} en el header de la solicitud");
            }
            string value = headers[name][0];
            if (value == null || value == "")
            {
                Error($"Debe mandar {name} en el header de la solicitud");
            }

            int result;
            if (!int.TryParse(value, out result))
            {
                Error($"El dato {name} debe ser un entero");
            }

            return result;
        }

        protected string GetHeaderString(AuthorizationFilterContext context, string name)
        {
            var rq = context.HttpContext.Request;
            var headers = rq.Headers;
            if (!headers.ContainsKey(name))
            {
                Error($"Debe mandar {name} en el header de la solicitud");
            }
            string value = headers[name][0];
            if (value == null || value == "")
            {
                Error($"Debe mandar {name} en el header de la solicitud");
            }
            return value;
        }

        public abstract List<BaseAttributeHeader> GetSwaggerHeaders();
        public abstract void OnAuthorization(AuthorizationFilterContext context);
    }
}

public class BaseAttributeHeader
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
    public bool IsRequired { get; set; }
}


public class SwaggerHeaders
{
    private static SwaggerHeaders _instance;
    public static SwaggerHeaders Instance
    {
        get
        {
            if (_instance == null) _instance = new SwaggerHeaders();
            return _instance;
        }
    }

    public List<BaseAttributeHeader> Headers { get; set; }
    public void Add(BaseAttributeHeader header)
    {
        if (Headers == null) Headers = new List<BaseAttributeHeader>();
        if (Headers.Any(x => x.Name == header.Name)) return;
        Headers.Add(header);
    }
}