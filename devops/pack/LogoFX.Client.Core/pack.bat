cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir net461\
robocopy ../../../../../src/Bin/netframework/Release net461 LogoFX.Client.Core.Platform.* /E
mkdir net6.0
robocopy ../../../../../src/Bin/net/Release net6.0 LogoFX.Client.Core.Platform.* /E
cd net6.0
rmdir /Q /S ref
cd ..
mkdir netcoreapp3.1
robocopy ../../../../../src/Bin/netcore/Release netcoreapp3.1 LogoFX.Client.Core.Platform.* /E
mkdir monoandroid403
robocopy ../../../../../src/Bin/android/Release monoandroid403 LogoFX.Client.Core.Platform.* /E
mkdir xamarin.ios10
robocopy ../../../../../src/Bin/ios/Release xamarin.ios10 LogoFX.Client.Core.Platform.* /E
cd ../../
nuget pack contents/LogoFX.Client.Core.nuspec -OutputDirectory ../../../output