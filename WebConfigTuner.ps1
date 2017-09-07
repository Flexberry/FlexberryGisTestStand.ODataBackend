Param(
	# New ConnectionStrings
	[String] $ConnectionStrings
)

$path = $MyInvocation.MyCommand.Path | split-path -parent
$webConfig = $path + '\ODataBackend\Web.config'
$doc = (Get-Content $webConfig) -as [Xml]

$root = $doc.get_DocumentElement();
$root.connectionStrings.add.connectionString = $ConnectionStrings;

$doc.Save($webConfig)
