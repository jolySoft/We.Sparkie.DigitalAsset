Feature: Basic Digital Asset Management
	In order to manage my audio files
	As an audiofile
	I want to upload, download, delete and list files

Scenario: Upload Audio File
    Given I have this wav file
    When I upload it
    Then I get the following details
        | Name                                           | Size       | Bit Depth | Sample Rate |
        | Ochre - Project Caelus - 06 Crowd of Stars.wav | 70,002,656 | 24        | 44100       |