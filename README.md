Dmp.DbEngineLaucher - launcher for postgresql server (support installation from remote official source and from local directory containing installation packages - use AllZipSourcesFromDirEngineInstalationResolver instead RemoteHttpSourcesEngineInstalationResolver, or implement your own IEngineInstalationResolver).

Examples of using:
<pre><code class="bash">// install postgre binaries to installPgSqlPath
// and initialize db cluster with database dir - pgDataPath (analog --pgdata)
// RemoteHttpSourcesEngineInstalationResolver -
// download binaries sources from official site (version can be specified as parameter)
// start server with default parameters: 
// host - "localhost"
// port - 5432,
// database superuser - postgres
// database password - postgres
// database encoding - UTF8
IDatabaseService dataBaseEngineService = new PostgreSqlDataBaseEngineLauncher(
new InstallationSettings(installPgSqlPath: "C:\\pgserverbinaries", pgDataPath: "C:\\pgdata"),
new RemoteHttpSourcesEngineInstalationResolver());
dataBaseEngineService.StartAsync().Wait();
dataBaseEngineService.StopAsync().Wait();
</code></pre>
local sources
<pre><code class="bash">// install postgre binaries to installPgSqlPath
// and initialize db cluster with database dir - pgDataPath (analog --pgdata)
// new AllZipSourcesFromDirEngineInstalationResolver("C:\\pgserverpackagesbinaries", "9.5.5-1") - 
// - use sources from dir C:\\pgserverpackagesbinaries
// directory have to contain files (one or more)
//postgresql-9.5.5-1-linux-binaries.tar.gz - for x86 linux
//postgresql-9.5.5-1-linux-x64-binaries.tar.gz - for x64 linux
//postgresql-9.5.5-1-osx-binaries.zip - for x64 osx
//postgresql-9.5.5-1-windows-binaries.zip - for x86 windows
//postgresql-9.5.5-1-windows-x64-binaries.zip - for x64 windows
IDatabaseService dataBaseEngineService = new PostgreSqlDataBaseEngineLauncher(
new InstallationSettings(installPgSqlPath: "C:\\pgserverbinaries", pgDataPath: "C:\\pgdata"),
new AllZipSourcesFromDirEngineInstalationResolver("C:\\pgserverpackagesbinaries", "9.5.5-1"));
</code></pre>
complex example
<pre><code class="bash">IDatabaseService dataBaseEngineService = new PostgreSqlDataBaseEngineLauncher(
	new SimpleSettingsProvider(
		new PostgreEngineStartupSettings(new ServerArrdessSettings("localhost", 29952),
			new DbCreationSettings("postgres", "postgres")), 
      new InstallationSettings(installPgSqlPath: "C:\\pgserverbinaries", pgDataPath: "C:\\pgdata"),
		new RunSettings(true, true)), new RemoteHttpSourcesEngineInstalationResolver(),
    new RuntimePlatformResolver(),
	  new UniversalResolver());
dataBaseEngineService.StartAsync().Wait();
dataBaseEngineService.StopAsync().Wait();
</code></pre>
