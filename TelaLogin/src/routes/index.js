import { createNativeStackNavigator } from "@react-navigation/native-stack";
import Welcome from "../pages/Welcome";
import SigIn from "../pages/SigIn";

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
        name="SigIn"
        component={SigIn}
        options={{ headerShown: false }}
        />
    </Stack.Navigator>
  );
}
