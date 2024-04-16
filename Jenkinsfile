pipeline {
    
    agent any

    tools {
        msbuild 'VS2022'
    }
    environment {
        publishDirectoryHost = "D:\\PublicWeb\\VEAM\\API"
        folderProfileHost = "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\VEAM_Backend\\API\\Properties\\PublishProfiles\\FolderProfile.pubxml"
        zipFileNameHost = "API.zip"
        zipFilePathHost = "\"${publishDirectoryHost}\\${zipFileNameHost}\""
    }

    stages {
        // stage('Checkout') {
        //     steps {
        //         // Checkout mã nguồn từ repository
        //         git branch: 'main', credentialsId: 'gitlabs-user-ci', url: 'http://192.168.60.133/product/VEAM.git'
        //     }
        // }
        stage('Build and Test') {
            steps {
                script {
                    bat "dotnet build -c Release API"
                    echo 'End Build API'
                    bat "dotnet test -c Release API"
                    echo 'End Test API'
                }
            }
        }
        stage('Publish') {
            steps {
                script {
                    bat "dotnet publish -c Release --output ${publishDirectoryHost}" 
                    powershell """
                        Compress-Archive -Path ${publishDirectoryHost}\\* -DestinationPath ${zipFilePathHost} -Force
                    """
                    echo 'End Publish API'
                }
            }
        }
    }    
}