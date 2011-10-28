<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SoLienLacTrucTuyen" generation="1" functional="0" release="0" Id="ff77913a-cf83-4d54-8e90-523145a6e709" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SoLienLacTrucTuyenGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SoLienLacTrucTuyen_WebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/LB:SoLienLacTrucTuyen_WebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="SoLienLacTrucTuyen_WebRole:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/MapSoLienLacTrucTuyen_WebRole:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="SoLienLacTrucTuyen_WebRole:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/MapSoLienLacTrucTuyen_WebRole:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="SoLienLacTrucTuyen_WebRole:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/MapSoLienLacTrucTuyen_WebRole:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="SoLienLacTrucTuyen_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/MapSoLienLacTrucTuyen_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SoLienLacTrucTuyen_WebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/MapSoLienLacTrucTuyen_WebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:SoLienLacTrucTuyen_WebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapSoLienLacTrucTuyen_WebRole:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRole/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapSoLienLacTrucTuyen_WebRole:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRole/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapSoLienLacTrucTuyen_WebRole:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRole/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapSoLienLacTrucTuyen_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSoLienLacTrucTuyen_WebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SoLienLacTrucTuyen_WebRole" generation="1" functional="0" release="0" software="D:\LuanVan\contactbook\trunk\Project_Engineering\SourceCode\SoLienLacTrucTuyen\SoLienLacTrucTuyen\bin\Debug\SoLienLacTrucTuyen.csx\roles\SoLienLacTrucTuyen_WebRole" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SoLienLacTrucTuyen_WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SoLienLacTrucTuyen_WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRoleInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="SoLienLacTrucTuyen_WebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="60c4c204-b297-4e10-8d4f-9cb8f94f42db" ref="Microsoft.RedDog.Contract\ServiceContract\SoLienLacTrucTuyenContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="85ec9ced-40fc-4684-9811-956625a76673" ref="Microsoft.RedDog.Contract\Interface\SoLienLacTrucTuyen_WebRole:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/SoLienLacTrucTuyen/SoLienLacTrucTuyenGroup/SoLienLacTrucTuyen_WebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>