rem TODO: Use common source for all version instances
SET version=2.2.2
rem TODO: Refactor using loop and automatic discovery
call deploy-single.bat LogoFX.Core %version% 
call deploy-single.bat LogoFX.Client.Core.Core %version%
call deploy-single.bat LogoFX.Client.Core %version%