using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Net.Http.Headers;
using MessagePack.Resolvers;
using XUCore.Serializer;

namespace XUCore.NetCore.Formatter
{

    public static class FormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddFormatters(builder, ApiFormatterOptionsConfiguration: null);
        }

        public static IMvcCoreBuilder AddFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return AddFormatters(builder, ApiFormatterOptionsConfiguration: null);
        }

        public static IMvcBuilder AddFormatters(this IMvcBuilder builder, Action<FormatterOptions> ApiFormatterOptionsConfiguration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var ApiFormatterOptions = new FormatterOptions();
            ApiFormatterOptionsConfiguration?.Invoke(ApiFormatterOptions);

            ApiFormatterOptions.Options.WithResolver(ApiFormatterOptions.FormatterResolver);

            foreach (var extension in ApiFormatterOptions.SupportedExtensions)
            {
                foreach (var contentType in ApiFormatterOptions.SupportedResponseFormatters.Keys)
                {
                    builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat(extension, new MediaTypeHeaderValue(contentType)));
                }
            }

            //builder.AddMvcOptions(options => options.InputFormatters.Add(new ApiInputFormatter(ApiFormatterOptions)));
            //builder.AddMvcOptions(options => options.OutputFormatters.Add(new ApiOutputFormatter(ApiFormatterOptions)));

            builder.AddMvcOptions(options =>
            {
                options.InputFormatters.Clear();
                options.InputFormatters.Add(new InputFormatter(ApiFormatterOptions));
            });
            builder.AddMvcOptions(options =>
            {
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new OutputFormatter(ApiFormatterOptions));
            });

            return builder;
        }

        public static IMvcCoreBuilder AddFormatters(this IMvcCoreBuilder builder, Action<FormatterOptions> ApiFormatterOptionsConfiguration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var ApiFormatterOptions = new FormatterOptions();
            ApiFormatterOptionsConfiguration?.Invoke(ApiFormatterOptions);

            ApiFormatterOptions.Options.WithResolver(ApiFormatterOptions.FormatterResolver);


            foreach (var extension in ApiFormatterOptions.SupportedExtensions)
            {
                foreach (var contentType in ApiFormatterOptions.SupportedResponseFormatters.Keys)
                {
                    builder.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat(extension, new MediaTypeHeaderValue(contentType)));
                }
            }

            //builder.AddMvcOptions(options => options.InputFormatters.Add(new ApiInputFormatter(ApiFormatterOptions)));
            //builder.AddMvcOptions(options => options.OutputFormatters.Add(new ApiOutputFormatter(ApiFormatterOptions)));

            builder.AddMvcOptions(options =>
            {
                options.InputFormatters.Clear();
                options.InputFormatters.Add(new InputFormatter(ApiFormatterOptions));
            });
            builder.AddMvcOptions(options =>
            {
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new OutputFormatter(ApiFormatterOptions));
            });

            return builder;
        }
    }
}
