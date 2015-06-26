param($installPath, $toolsPath, $package, $project)

function Resolve-ProjectName {
    param(
        [parameter(ValueFromPipelineByPropertyName = $true)]
        [string[]]$ProjectName
    )
    
    if($ProjectName) {
        $projects = Get-Project $ProjectName
    }
    else {
        # All projects by default
        $projects = Get-Project
    }
    
    $projects
}


function Get-MSBuildProject {
    param(
        [parameter(ValueFromPipelineByPropertyName = $true)]
        [string[]]$ProjectName
    )
    Process {
        (Resolve-ProjectName $ProjectName) | % {
            $path = $_.FullName
            @([Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($path))[0]
        }
    }
}

Set-Location (Join-Path $project.FullName "..")
$relativePath = Get-Item (Join-Path $toolsPath "KindOfMagic.targets") | Resolve-Path -Relative

$buildProject = Get-MSBuildProject $project.Name

$buildProject.Xml.Imports | Where-Object { $_.Project -match "KindOfMagic" } | foreach-object {
    $buildProject.Xml.RemoveChild($_) 
}

$buildProject.Xml.AddImport($relativePath)

$project.Save()