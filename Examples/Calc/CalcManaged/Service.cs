﻿using System.Text;
using System.Threading.Tasks;

namespace Calc.Managed
{
    internal class Service
    {
        public Task<Response> Invoke(Request request)
        {
            var response = new Response();
            var text = new StringBuilder();
            text.Append(request.X);
            text.Append(' ');

            switch (request.Op)
            {
                case Operation.Add:
                    response.Z = request.X + request.Y;
                    text.Append('+');
                    break;

                case Operation.Subtract:
                    response.Z = request.X - request.Y;
                    text.Append('-');
                    break;

                case Operation.Multiply:
                    response.Z = request.X * request.Y;
                    text.Append('*');
                    break;

                case Operation.Divide:
                    response.Z = request.X / request.Y;
                    text.Append('/');
                    break;
            }

            text.Append(' ');
            text.Append(request.Y);
            text.Append(" = ");
            response.Text = text.ToString();

            return Task.FromResult(response);
        }
    }
}
