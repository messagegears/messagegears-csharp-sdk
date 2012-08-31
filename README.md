MessageGears C# SDK
===================

This library is a comprehensive wrapper around the MessaegGears Web Services APIs.  It also provides a Amazon SQS message consumer for those customers who subscribe to the real-time event feed service and a streaming XML parser for downloading and processing the daily account activity files.

Installation Instructions
-------------------------

### Download

Click the download link above and retrieve the 3 libraries to be referenced by your project:

   * AWSSDK.dll
   * log4net.dll
   * MessageGearsSDK.dll

### Logging Configuration 

The MessageGears SDK utilizes log4net to provide application logging. 

To enable logging for the MessageGears SDK add the following to your applications App.config file:

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

#### More Examples

Examples are provided for each API endpoint. See http://messagegears.com/docs/web-services/ for more information.
