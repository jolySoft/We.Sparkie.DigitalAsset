Feature: Upload Digital Asset
	In order to manage my audio files
	As an audiofile
	I want to upload files

Scenario: Data is read from Audio File when uploaded
    Given I have this wav file
    When I upload it
    Then I get the following details
        | Name                                           | Size      | Bit Depth | Sample Rate | Content Type |
        | Ochre - Project Caelus - 06 Crowd of Stars.wav | 3,458,286 | 24        | 44100       | audio/wav    |

Scenario: Audio file is stored when uploaded
    Given I have this wav file
    When I upload it
    Then it is put in storage

Scenario: Audio metadata is also stored
    Given I have this wav file
    When I upload it
    Then its metadata is stored

Scenario: Audio stream is reset once metadata is extracted
    Given I have this wav file
    When I upload it
    Then the stream is reset