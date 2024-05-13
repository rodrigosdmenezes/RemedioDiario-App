import { createNativeStackNavigator } from "@react-navigation/native-stack";
import Welcome from "../pages/Welcome";
import SignIn from "../pages/SignIn";
import SignUp from "../pages/SignUp";
import Reset from "../pages/Reset/Reset";
import Menu from "../pages/Menu/Menu";
import ListMed from "../pages/ListMed/ListMed"

const Stack = createNativeStackNavigator();

export default function Routes() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="Welcome" 
        component={Welcome}
        options={{ headerShown: false }}
        />

      <Stack.Screen 
        name="SignIn"
        component={SignIn}
        options={{ headerShown: false }}
        />
      <Stack.Screen 
        name="SignUp"
        component={SignUp}
        options={{ headerShown: false }}
        />
        <Stack.Screen 
        name="Reset"
        component={Reset}
        options={{ headerShown: false }}
        />  
        <Stack.Screen 
        name="Menu"
        component={Menu}
        options={{ headerShown: false }}
        />    
        <Stack.Screen 
        name="ListMed"
        component={ListMed}
        options={{ headerShown: false }}
        />   
    </Stack.Navigator>
  );
}
