package com.example.staterecognition;
import android.content.Context;
import android.view.Gravity;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;

public class StatesChart {
    private Context context;

    private LinearLayout layout;
    private final int displayedStatesNumber;
    private ArrayList<TextView> statesChart;

    public StatesChart(Context context, LinearLayout layout, int displayedStatesNumber) {
        this.context = context;
        this.layout = layout;
        this.displayedStatesNumber = displayedStatesNumber;

        //statesChart = new TextView[displayedStatesNumber];
        statesChart = new ArrayList<>();


        /*
        for (int i = 0; i < displayedStatesNumber; i++) {
            TextView newState = new TextView(context);

            //LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(5, 150);
            newState.setGravity(Gravity.BOTTOM);
            newState.setBackgroundColor(0xFF008505);

            //lLayout.addView(newState, layoutParams);
            layout.addView(newState);

            //newState.setLayoutParams(new LinearLayout.LayoutParams(5, i * 5));
            newState.setLayoutParams(new LinearLayout.LayoutParams(5, 0));


            statesChart.add(newState);
            //statesChart[i] = newState;
        }*/
    }

    public void addState(int state) {
        if (statesChart.size()!= 0) {
            layout.removeView(statesChart.get(0));
            TextView tvState = statesChart.remove(0);
            //layout.removeView(tvState);
        }

        TextView newState = new TextView(context);
        newState.setGravity(Gravity.RIGHT);
        newState.setBackgroundColor(0xFF008505);

        layout.addView(newState);

        newState.setLayoutParams(new LinearLayout.LayoutParams(5, 200 * (state + 1)));

        statesChart.add(newState);

    }
}
