cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir netstandard2.0\
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Modularity.* /E
cd ../../
nuget pack contents/LogoFX.Client.Modularity.nuspec -OutputDirectory ../../../output