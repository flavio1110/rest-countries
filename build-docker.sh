rm -rf docker-publish

dotnet build 

cd src/RestCountries.Api/

dotnet publish -c Release -o ../../docker-publish --no-restore -v minimal

cd ../../

cp Dockerfile docker-publish 

docker build -t rest-country docker-publish

rm -rf docker-publish docker-publish 