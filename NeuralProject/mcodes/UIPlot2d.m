%

function result=UIPlot2d()

reqstr={'path','БъЬт','xlabel','ylabel','format','legend'};
ttl=['UIPlot2d'];

defaultstring={'D:\.txt' '' '' '' 'r*' ''};

out=inputdlg(reqstr,ttl,[1,35],defaultstring);

a=dlmread(out{1});
sz=size(a);

if sz(2)==1
    plot(1:length(a),a,out{5});
else
    plot(a(:,1),a(:,2),out{5});
end

grid on
title(out{2})
xlabel(out{3})
ylabel(out{4})
legend(out{6})

result=1;