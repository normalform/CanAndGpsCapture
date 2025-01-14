; -----------------------------------------------------------------------------
;  Copyright (C) 2025 Your Company Name
;  This program is free software: you can redistribute it and/or modify
;  it under the terms of the GNU General Public License as published by
;  the Free Software Foundation, either version 3 of the License or any later version.
; -----------------------------------------------------------------------------

[Setup]
AppName=CagCap
AppVersion={#SetupVersion}
DefaultDirName={code:GetInstallDir}
DefaultGroupName=CagCap
OutputDir=..\
OutputBaseFilename=CagCapSetup
PrivilegesRequired=admin
Compression=lzma2
SolidCompression=yes

[Files]
Source: "..\src\CagCap\bin\Release\net8.0\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\src\CagCap\CagCapSettings.json"; DestDir: "{code:GetConfigDir}"; Flags: ignoreversion

[Icons]
Name: "{group}\CagCap"; Filename: "{app}\CagCap.exe"
Name: "{group}\Uninstall CagCap"; Filename: "{uninstallexe}"

[Code]
var
  InstallForAllUsers: Boolean;
  PageInstallationType: TInputOptionWizardPage;

procedure InitializeWizard;
  begin
    PageInstallationType := CreateInputOptionPage(wpWelcome, 'Installation Type', 'Select the installation type:', 'Choose whether to install for all users or just for the current user.', True, False);
    PageInstallationType.Add('Install for all users (admin only)');
    PageInstallationType.Add('Install for current user only');
    PageInstallationType.Values[1] := True; // Default to "Install for current user only";
  end;
  
  function GetInstallDir(Default: String): String;
  begin
    if InstallForAllUsers then
      Result := ExpandConstant('{commonpf64}\CagCap')
    else
      Result := ExpandConstant('{userpf}\CagCap');
  end;
  
  function GetConfigDir(Default: String): String;
  begin
    if InstallForAllUsers then
      Result := ExpandConstant('{commonappdata}\CagCap')
    else
      Result := ExpandConstant('{userappdata}\CagCap');
  end;
    
  function NextButtonClick(CurPageID: Integer): Boolean;
  begin
    if CurPageID = PageInstallationType.ID then
    begin
      InstallForAllUsers := PageInstallationType.Values[0];
      WizardForm.DirEdit.Text := GetInstallDir('');
    end;
    Result := True;
  end;