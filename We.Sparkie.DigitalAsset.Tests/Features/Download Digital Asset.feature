Feature: Download and list Digital Asset
	In order to manage my audio files
	As an audiofile
	I want to download and list files

Scenario: Dowload an asset from right location
	Given I have uploaded this asset
        | Id                                   | Name                                           | Location                             |
        | 50FC9308-7A50-4B74-9702-B999AFAEE4B0 | Ochre - Project Caelus - 06 Crowd of Stars.wav | D9AE1DF8-99EE-4F5C-92D8-10F7ADF6C4AB |
    When I download an asset with this id 50FC9308-7A50-4B74-9702-B999AFAEE4B0
    Then it is download from this location D9AE1DF8-99EE-4F5C-92D8-10F7ADF6C4AB
    And the audio data is populated

Scenario: List out assets
    Given I have uploaded these assets
        | Id                                   | Name                                           | Location                             | Size       | Bit Depth | Sample Rate |
        | 50FC9308-7A50-4B74-9702-B999AFAEE4B0 | Ochre - Project Caelus - 06 Crowd of Stars.wav | D9AE1DF8-99EE-4F5C-92D8-10F7ADF6C4AB | 3,458,286  | 24        | 44100       |
        | F9700558-A488-40BD-B572-E301C4D5286D | Brian Adams - Something really bad.wav         | 4AFE0223-6007-4C0D-898D-6BB0B89328F6 | 7,0345,462 | 16        | 44100       |
    When I list my assets
    Then I get this list
        | Id                                   | Name                                           | Size       | Bit Depth | Sample Rate |
        | 50FC9308-7A50-4B74-9702-B999AFAEE4B0 | Ochre - Project Caelus - 06 Crowd of Stars.wav | 3,458,286  | 24        | 44100       |
        | F9700558-A488-40BD-B572-E301C4D5286D | Brian Adams - Something really bad.wav         | 7,0345,462 | 16        | 44100       |
