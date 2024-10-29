# Revit Paramameters Add-In  

Revit Parameters is an add-in for Autodesk Revit that allows users to enter a parameter name and value to search for elements, select them, and easily isolate them.

## Features 
**Custom Search**: Find elements in your Revit model using the parameter name and value.  
**Quick Selection**: Automatically select all elements matching the search criteria.  
**Quick Isolation**: Isolate the selected elements with a single click, making them easy to view and edit.   

## Requirements 

Autodesk Revit 2020

## Installation 

1. Download the ZIP file of the add-in.   

2. Right-click on the ZIP file, select Properties, and in the General tab, check Unblock if available. After extracting, also check and unblock the Parameters.dll file in the same way.

3. Extract the contents of the ZIP file.  

4. Copy the `Parameters.dll` and `Parameters.addin` file to the add-ins Revit folder  
	Typically located at:  
	C:\Users\<User>\AppData\Roaming\Autodesk\Revit\Addins\2020  

5. Start Revit. 

## Usage  
1. Open your project in Revit. 
2. In the ribbon, navigate to the `Parameters` tab and select `Parameter Scanner`.
3. Enter the parameter name you wish to search for.
4. Enter the parameter value. 
5. Click the `Select` button to select elements matching criteria.
6. Click `Isolate Elements` to isolate selected elements.
