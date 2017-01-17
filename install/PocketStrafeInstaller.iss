; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "PocketStrafe PC"
#define MyAppVersion "1.2"
#define MyAppPublisher "Cool Font LLC"
#define MyAppURL "http://www.pocketstrafe.com"
#define MyAppExeName "CoolFontWin.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D3A3322D-5E08-4624-8D84-B19D345CC7D6}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename=PocketStrafeSetup
Compression=lzma
SolidCompression=yes
DisableReadyPage=yes
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
SignTool=signtool

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
Source: "F:\Program Files (x86)\Microsoft Visual Studio 14.0\SDK\Bootstrapper\Packages\BonjourForWindows\Bonjour64.msi"; DestDir: "{tmp}"; Flags: deleteafterinstall
Source: "E:\Documents\Developer\CoolFontWin\CoolFontWin\bin\x64\Release\CoolFontWin.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Documents\Developer\CoolFontWin\CoolFontWin\bin\x64\Release\*"; Excludes: "*.pdb, *.xml, *.manifest, *vshost*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\Bonjour64.msi"""; Check: IsBonjourNotInstalled; Description: "Install Bonjour (required)"
Filename: "{app}\scpvbus\devcon.exe"; Parameters: "install ScpVBus.inf Root\ScpVBus"; Description: "Install virtual Xbox controllers"; Flags: runhidden runascurrentuser;
Filename: "{app}\vJoy\vJoySetup.exe"; Description: "Install virtual joystick (optional)"; Flags: postinstall unchecked
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{app}\scpvbus\devcon.exe"; Parameters: "remove Root\ScpVBus"; Flags: runhidden runascurrentuser;

[Code] 
function IsBonjourNotInstalled():Boolean;
begin
    Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Apple Inc.\Bonjour');
end;

