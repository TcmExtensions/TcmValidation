## Introduction #

TcmValidation is a set of console applications which allow resolving component links and retrieve dynamic component presentations from a Tridion installation.

Because the applications are self-contained they can be quickly used to verify a Tridion presentation server installation.

The advantage of this is that this allows isolating testing of the Tridion installation only, irregardless of any application configuration issues.

### Download #

Pre-compiled binary versions available for download can be found here:
[TcmValidation on Google Drive](https://drive.google.com/folderview?id=0B7HbFVRJj_UnWk8zcUpVNlpiS1U&usp=sharing)


### Details #

TcmValidation consists of a series of applications with a shared codebase.

Each application is compiled against a specific Tridion version / bitness and Tridion JVM model.

The component link and dynamic component presentation to be retrieved can be configured in each configuration file:

    <?xml version="1.0"?>
    <configuration>
        <appSettings>
            <add key="PublicationUri" value="tcm:0-233-1"/>
            <add key="LinkUri" value="tcm:233-193779"/>
            <add key="BrokerUri" value="tcm:233-214214"/>
        </appSettings>
    </configuration>

Additionally for the native CodeMesh JVM versions, the utility also displays which JVM is actually used:

    CodeMesh using Java: C:\Program Files\Java\jre6\bin\server\jvm.dll

The applications available are:

<dl>
<dt>Tridion 2009 COM</dt>
<dd>32-bit interface in .net 2 using deprecated Tridion 2009 COM+</dd>
<dt>Tridion 2009 Native</dt>
<dd>32-bit interface in .net 2 using CodeMesh JVM</dd>
<dt>Tridion 2011 Native</dt>
<dd>Both 32-bit / 64-bit interface in .net 2 using CodeMesh JVM</dd>
<dt>Tridion 2013 Native</dt>
<dd>Both 32-bit / 64-bit interface in .net 4 using CodeMesh JVM</dd>
</dl>

[![githalytics.com alpha](https://cruel-carlota.pagodabox.com/cb439853d159fe1de33e3197f1caf6f7 "githalytics.com")](http://githalytics.com/github.com/TcmExtensions)
