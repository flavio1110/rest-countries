FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY . /app
EXPOSE 80
ENTRYPOINT ["dotnet", "RestCountries.Api.dll"]