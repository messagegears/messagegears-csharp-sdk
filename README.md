MessageGears C# SDK
===================

This library is a comprehensive wrapper around the MessaegGears Web Services APIs.  It also provides a Amazon SQS message consumer for those customers who subscribe to the real-time event feed service.  It also provides a streaming XML parser for downloading and processing the daily account activity files.

Installation Instructions
-------------------------
1. Click the download link above and retrieve the 3 libraries to be referenced by your project:
   AWSSDK.dll
   log4net.dll
   MessageGearsSDK.dll

2) Create a C# application with the following App.config file:

<configuration>
    <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,Log4net"/>
    </configSections>
    <log4net>
        <root>
            <level value="INFO" />
            <appender-ref ref="LogFileAppender" />
        </root>
        <appender name="LogFileAppender" type="log4net.Appender.RollingFileAppender" >
            <file value="messagegears.log" />
            <appendToFile value="true" />
            <maximumFileSize value="50MB" />
            <maxSizeRollBackups value="2" />
            <layout type="log4net.Layout.PatternLayout">
                <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
            </layout>
        </appender>
    </log4net>
</configuration>

3) Refer to the example application to build your application: https://github.com/messagegears/messagegears-csharp-examples
